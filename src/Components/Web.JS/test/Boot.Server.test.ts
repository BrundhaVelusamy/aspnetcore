// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

import { afterEach, expect, jest, test } from '@jest/globals';
import type { HttpRequest } from '@microsoft/signalr';
import type { CircuitStartOptions } from '../src/Platform/Circuits/CircuitStartOptions';
import { LogLevel } from '../src/Platform/Logging/Logger';

afterEach(() => {
  jest.restoreAllMocks();
});

async function loadBlazorStart() {
  const script = document.createElement('script');
  script.setAttribute('autostart', 'false');
  jest.spyOn(document, 'currentScript', 'get').mockReturnValue(script);
  await import('../src/Boot.Server');
  const { Blazor } = await import('../src/GlobalExports');
  if (!Blazor.start) {
    throw new Error('Blazor.start was not registered.');
  }
  return Blazor.start;
}

const options: Partial<CircuitStartOptions> = {
  initializers: { beforeStart: [], afterStarted: [] },
  logLevel: LogLevel.None,
};

test('rejects manual startup when the hub refuses the first circuit', () => jest.isolateModulesAsync(async () => {
  const { HubConnection, HubConnectionState } = await import('@microsoft/signalr');
  jest.spyOn(HubConnection.prototype, 'start').mockResolvedValue(undefined);
  jest.spyOn(HubConnection.prototype, 'state', 'get').mockReturnValue(HubConnectionState.Connected);
  const invoke = jest.spyOn(HubConnection.prototype, 'invoke').mockResolvedValue(null);

  const start = await loadBlazorStart();
  await expect(start({
    ...options,
    configureSignalR: builder => {
      builder.withUrl(new URL('_blazor', document.baseURI).href);
    },
  })).rejects.toThrow('Failed to start the circuit.');
  expect(invoke).toHaveBeenCalledWith('StartCircuit', expect.any(String), expect.any(String), '[]', '');
}));

test('rejects manual startup when SignalR negotiation returns 401', () => jest.isolateModulesAsync(async () => {
  const { HttpClient, HttpError, LogLevel: SignalRLogLevel } = await import('@microsoft/signalr');
  const send = jest.fn(async (_request: HttpRequest) => {
    throw new HttpError('Unauthorized', 401);
  });
  class UnauthorizedHttpClient extends HttpClient {
    send = send;
  }

  const start = await loadBlazorStart();
  await expect(start({
    ...options,
    configureSignalR: builder => {
      builder.withUrl(new URL('_blazor', document.baseURI).href, { httpClient: new UnauthorizedHttpClient() });
      builder.configureLogging(SignalRLogLevel.None);
    },
  })).rejects.toMatchObject({ errorType: 'FailedToNegotiateWithServerError' });
  expect(send).toHaveBeenCalledWith(expect.objectContaining({ method: 'POST', url: expect.stringContaining('_blazor/negotiate') }));
}));
