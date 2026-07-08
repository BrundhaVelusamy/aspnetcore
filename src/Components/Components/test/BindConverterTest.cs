// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Globalization;
using System.Text.Json;

namespace Microsoft.AspNetCore.Components;

// This is some basic coverage, it's not in depth because there are many many APIs here
// and they mostly call through to CoreFx. We don't want to test the globalization details
// of .NET in detail where we can avoid it.
//
// Instead there's a sampling of things that have somewhat unique behavior or semantics.
public class BindConverterTest
{
    [Fact]
    public void FormatValue_Bool()
    {
        // Arrange
        var value = true;
        var expected = true;

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_Bool_Generic()
    {
        // Arrange
        var value = true;
        var expected = true;

        // Act
        var actual = BindConverter.FormatValue<bool>(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_NullableBool()
    {
        // Arrange
        var value = (bool?)true;
        var expected = true;

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_NullableBool_Generic()
    {
        // Arrange
        var value = true;
        var expected = true;

        // Act
        var actual = BindConverter.FormatValue<bool?>(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_NullableBoolNull()
    {
        // Arrange
        var value = (bool?)null;
        var expected = (bool?)null;

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_NullableBoolNull_Generic()
    {
        // Arrange
        var value = (bool?)null;
        var expected = (bool?)null;

        // Act
        var actual = BindConverter.FormatValue<bool?>(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_Int()
    {
        // Arrange
        var value = 17;
        var expected = "17";

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_Int_Generic()
    {
        // Arrange
        var value = 17;
        var expected = "17";

        // Act
        var actual = BindConverter.FormatValue<int>(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_NullableInt()
    {
        // Arrange
        var value = (int?)17;
        var expected = "17";

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_NullableInt_Generic()
    {
        // Arrange
        var value = 17;
        var expected = "17";

        // Act
        var actual = BindConverter.FormatValue<int?>(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_DateTime()
    {
        // Arrange
        var value = DateTime.Now;
        var expected = value.ToString(CultureInfo.CurrentCulture);

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_DateTime_Format()
    {
        // Arrange
        var value = DateTime.Now;
        var expected = value.ToString("MM-yyyy", CultureInfo.InvariantCulture);

        // Act
        var actual = BindConverter.FormatValue(value, "MM-yyyy", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_DateOnly()
    {
        // Arrange
        var value = DateOnly.FromDateTime(DateTime.Now);
        var expected = value.ToString(CultureInfo.CurrentCulture);

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_DateOnly_Format()
    {
        // Arrange
        var value = DateOnly.FromDateTime(DateTime.Now);
        var expected = value.ToString("MM-yyyy", CultureInfo.InvariantCulture);

        // Act
        var actual = BindConverter.FormatValue(value, "MM-yyyy", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_TimeOnly()
    {
        // Arrange
        var value = TimeOnly.FromDateTime(DateTime.Now);
        var expected = value.ToString(CultureInfo.CurrentCulture);

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_TimeOnly_Format()
    {
        // Arrange
        var value = TimeOnly.FromDateTime(DateTime.Now);
        var expected = value.ToString("HH:mm", CultureInfo.InvariantCulture);

        // Act
        var actual = BindConverter.FormatValue(value, "HH:mm", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_Enum()
    {
        // Arrange
        var value = SomeLetters.A;
        var expected = value.ToString();

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_Enum_OutOfRange()
    {
        // Arrange
        var value = SomeLetters.A + 3;
        var expected = value.ToString();

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_NullableEnum()
    {
        // Arrange
        var value = (SomeLetters?)null;

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void FormatValue_TypeConverter()
    {
        // Arrange
        var value = new Person()
        {
            Name = "Glenn",
            Age = 47,
        };

        var expected = JsonSerializer.Serialize(value);

        // Act
        var actual = BindConverter.FormatValue(value);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TryConvertTo_Guid_Valid()
    {
        // Arrange
        var expected = Guid.NewGuid();
        var incomingValue = expected.ToString();

        // Act
        var successfullyConverted = BindConverter.TryConvertTo<Guid>(incomingValue, CultureInfo.CurrentCulture, out var actual);

        // Assert
        Assert.Equal(expected, actual);
        Assert.True(successfullyConverted);
    }

    [Theory]
    [InlineData("invalidguid")]
    [InlineData("")]
    [InlineData(null)]
    public void TryConvertTo_Guid_Invalid(string incomingValue)
    {
        // Act
        var successfullyConverted = BindConverter.TryConvertTo<Guid>(incomingValue, CultureInfo.CurrentCulture, out var actual);

        // Assert
        Assert.False(successfullyConverted);
        Assert.Equal(Guid.Empty, actual);
    }

    [Fact]
    public void TryConvertTo_NullableGuid_Valid()
    {
        // Arrange
        var expected = Guid.NewGuid();
        var incomingValue = expected.ToString();

        // Act
        var successfullyConverted = BindConverter.TryConvertTo<Guid?>(incomingValue, CultureInfo.CurrentCulture, out var actual);

        // Assert
        Assert.True(successfullyConverted);
        Assert.Equal(expected, actual.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TryConvertTo_NullableGuid_ValidEmptyOrNull(string incomingValue)
    {
        // Act
        var successfullyConverted = BindConverter.TryConvertTo<Guid?>(incomingValue, CultureInfo.CurrentCulture, out var actual);

        // Assert
        Assert.True(successfullyConverted);
        Assert.Null(actual);
    }

    [Fact]
    public void TryConvertTo_NullableGuid__Invalid()
    {
        // Arrange
        var value = "invalidguid";

        // Act
        var successfullyConverted = BindConverter.TryConvertTo<Guid?>(value, CultureInfo.CurrentCulture, out var actual);

        // Assert
        Assert.False(successfullyConverted);
        Assert.Null(actual);
    }

    [Fact]
    public void FormatValue_Decimal_Format()
    {
        var value = 20129.99m;
        var expected = value.ToString("N2", CultureInfo.InvariantCulture);

        var actual = BindConverter.FormatValue(value, "N2", CultureInfo.InvariantCulture);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(0, "N2", "0.00")]
    [InlineData(1234, "N0", "1,234")]
    [InlineData(-1234, "N0", "-1,234")]
    [InlineData(20129.99, "N2", "20,129.99")]
    [InlineData(20129.99, "C2", "¤20,129.99")]
    [InlineData(0.85, "P0", "85 %")]
    [InlineData(1234.567, "0.####", "1234.567")]
    [InlineData(0, "0", "0")]
    [InlineData(0, "0.00", "0.00")]
    [InlineData(1234567890.12, "#,##0.00", "1,234,567,890.12")]
    public void FormatValue_Decimal_WithFormat(double input, string format, string expected)
    {
        var value = (decimal)input;

        var actual = BindConverter.FormatValue(value, format, CultureInfo.InvariantCulture);

        Assert.Equal(expected, actual);
        Assert.Equal(value.ToString(format, CultureInfo.InvariantCulture), actual);
    }

    [Fact]
    public void FormatValue_Decimal_WithFormat_Nullable()
    {
        decimal? value = 20129.99m;

        var actual = BindConverter.FormatValue(value, "N2", CultureInfo.InvariantCulture);

        Assert.Equal("20,129.99", actual);
    }

    [Fact]
    public void FormatValue_Decimal_WithFormat_NullableNull()
    {
        decimal? value = null;

        var actual = BindConverter.FormatValue(value, "N2", CultureInfo.InvariantCulture);

        Assert.Null(actual);
    }

    [Fact]
    public void FormatValue_Decimal_WithFormat_NullFormat()
    {
        var value = 20129.99m;

        var actual = BindConverter.FormatValue(value, format: null, CultureInfo.InvariantCulture);

        Assert.Equal(value.ToString(CultureInfo.InvariantCulture), actual);
    }

    [Fact]
    public void FormatValue_Decimal_WithFormat_NullCulture_UsesCurrentCulture()
    {
        var value = 20129.99m;
        var savedCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var actual = BindConverter.FormatValue(value, "N2", culture: null);

            Assert.Equal("20,129.99", actual);
        }
        finally
        {
            CultureInfo.CurrentCulture = savedCulture;
        }
    }

    [Theory]
    [InlineData(20129.99, "N2")]
    [InlineData(1234.567, "0.####")]
    [InlineData(1234.5, "0.0")]
    [InlineData(0, "N0")]
    [InlineData(-1234, "N0")]
    public void FormatValue_Decimal_RoundTrip(double value, string format)
    {
        var decimalValue = (decimal)value;
        var invariant = CultureInfo.InvariantCulture;

        var formatted = BindConverter.FormatValue(decimalValue, format, invariant);
        var converted = decimal.Parse(formatted, NumberStyles.Number, invariant);

        Assert.Equal(decimalValue, converted);
    }

    [Fact]
    public void FormatValue_Decimal_WithFormat_DeDECulture()
    {
        var value = 20129.99m;
        var culture = CultureInfo.GetCultureInfo("de-DE");

        var actual = BindConverter.FormatValue(value, "N2", culture);

        Assert.Equal("20.129,99", actual);
    }

    [Fact]
    public void FormatValue_Decimal_WithFormat_FrFRCulture()
    {
        var value = 20129.99m;
        var culture = CultureInfo.GetCultureInfo("fr-FR");

        var actual = BindConverter.FormatValue(value, "N2", culture);

        var expected = value.ToString("N2", culture);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FormatValue_Decimal_WithFormat_JaJPCulture()
    {
        var value = 20129.99m;
        var culture = CultureInfo.GetCultureInfo("ja-JP");

        var actual = BindConverter.FormatValue(value, "N2", culture);

        Assert.Contains("20,129.99", actual);
    }

    [Fact]
    public void FormatValue_Float_WithFormat()
    {
        var value = 1234.5678f;

        var actual = BindConverter.FormatValue(value, "N2", CultureInfo.InvariantCulture);

        Assert.Equal("1,234.57", actual);
    }

    [Fact]
    public void FormatValue_Float_WithFormat_Nullable()
    {
        float? value = 1234.5678f;

        var actual = BindConverter.FormatValue(value, "N2", CultureInfo.InvariantCulture);

        Assert.Equal("1,234.57", actual);
    }

    [Fact]
    public void FormatValue_Float_WithFormat_NullableNull()
    {
        float? value = null;

        var actual = BindConverter.FormatValue(value, "N2", CultureInfo.InvariantCulture);

        Assert.Null(actual);
    }

    [Fact]
    public void FormatValue_Float_WithFormat_Percent()
    {
        var value = 0.85f;

        var actual = BindConverter.FormatValue(value, "P1", CultureInfo.InvariantCulture);

        Assert.Equal(value.ToString("P1", CultureInfo.InvariantCulture), actual);
    }

    [Fact]
    public void FormatValue_Float_WithFormat_EmptyString()
    {
        var value = 0f;

        var actual = BindConverter.FormatValue(value, "", CultureInfo.InvariantCulture);

        Assert.Equal("0", actual);
    }

    [Fact]
    public void FormatValue_Float_WithFormat_NullFormat()
    {
        var value = 1234.5f;

        var actual = BindConverter.FormatValue(value, format: null, CultureInfo.InvariantCulture);

        Assert.Equal("1234.5", actual);
    }

    [Fact]
    public void FormatValue_Double_WithFormat()
    {
        var value = 1234.5678;

        var actual = BindConverter.FormatValue(value, "N2", CultureInfo.InvariantCulture);

        Assert.Equal("1,234.57", actual);
    }

    [Fact]
    public void FormatValue_Double_WithFormat_Nullable()
    {
        double? value = 1234.5678;

        var actual = BindConverter.FormatValue(value, "N2", CultureInfo.InvariantCulture);

        Assert.Equal("1,234.57", actual);
    }

    [Fact]
    public void FormatValue_Double_WithFormat_NullableNull()
    {
        double? value = null;

        var actual = BindConverter.FormatValue(value, "N2", CultureInfo.InvariantCulture);

        Assert.Null(actual);
    }

    [Fact]
    public void FormatValue_Double_WithFormat_HighPrecision()
    {
        var value = 3.14159265358979;

        var actual = BindConverter.FormatValue(value, "F5", CultureInfo.InvariantCulture);

        Assert.Equal("3.14159", actual);
    }

    [Fact]
    public void FormatValue_Double_WithFormat_Scientific()
    {
        var value = 1234567890.0;

        var actual = BindConverter.FormatValue(value, "E2", CultureInfo.InvariantCulture);

        Assert.Equal("1.23E+009", actual);
    }

    [Fact]
    public void FormatValue_Double_WithFormat_NullFormat()
    {
        var value = 1234.5678;

        var actual = BindConverter.FormatValue(value, format: null, CultureInfo.InvariantCulture);

        Assert.Equal("1234.5678", actual);
    }

    [Fact]
    public void FormatValue_Int_WithFormat()
    {
        var value = 1234;

        var actual = BindConverter.FormatValue(value, "N0", CultureInfo.InvariantCulture);

        Assert.Equal("1,234", actual);
    }

    [Fact]
    public void FormatValue_Int_WithFormat_Nullable()
    {
        int? value = 1234;

        var actual = BindConverter.FormatValue(value, "N0", CultureInfo.InvariantCulture);

        Assert.Equal("1,234", actual);
    }

    [Fact]
    public void FormatValue_Int_WithFormat_NullableNull()
    {
        int? value = null;

        var actual = BindConverter.FormatValue(value, "N0", CultureInfo.InvariantCulture);

        Assert.Null(actual);
    }

    [Fact]
    public void FormatValue_Int_WithFormat_Hex()
    {
        var value = 255;

        var actual = BindConverter.FormatValue(value, "X", CultureInfo.InvariantCulture);

        Assert.Equal("FF", actual);
    }

    [Fact]
    public void FormatValue_Int_WithFormat_NullFormat()
    {
        var value = 1234;

        var actual = BindConverter.FormatValue(value, format: null, CultureInfo.InvariantCulture);

        Assert.Equal("1234", actual);
    }

    [Fact]
    public void FormatValue_Long_WithFormat()
    {
        var value = 1234567890123L;

        var actual = BindConverter.FormatValue(value, "N0", CultureInfo.InvariantCulture);

        Assert.Equal("1,234,567,890,123", actual);
    }

    [Fact]
    public void FormatValue_Long_WithFormat_Nullable()
    {
        long? value = 1234567890123L;

        var actual = BindConverter.FormatValue(value, "N0", CultureInfo.InvariantCulture);

        Assert.Equal("1,234,567,890,123", actual);
    }

    [Fact]
    public void FormatValue_Long_WithFormat_NullableNull()
    {
        long? value = null;

        var actual = BindConverter.FormatValue(value, "N0", CultureInfo.InvariantCulture);

        Assert.Null(actual);
    }

    [Fact]
    public void FormatValue_Long_WithFormat_NullFormat()
    {
        var value = 1234567890123L;

        var actual = BindConverter.FormatValue(value, format: null, CultureInfo.InvariantCulture);

        Assert.Equal("1234567890123", actual);
    }

    [Fact]
    public void FormatValue_Short_WithFormat()
    {
        var value = (short)1234;

        var actual = BindConverter.FormatValue(value, "N0", CultureInfo.InvariantCulture);

        Assert.Equal("1,234", actual);
    }

    [Fact]
    public void FormatValue_Short_WithFormat_Nullable()
    {
        short? value = 1234;

        var actual = BindConverter.FormatValue(value, "N0", CultureInfo.InvariantCulture);

        Assert.Equal("1,234", actual);
    }

    [Fact]
    public void FormatValue_Short_WithFormat_NullableNull()
    {
        short? value = null;

        var actual = BindConverter.FormatValue(value, "N0", CultureInfo.InvariantCulture);

        Assert.Null(actual);
    }

    [Fact]
    public void FormatValue_Short_WithFormat_NullFormat()
    {
        var value = (short)1234;

        var actual = BindConverter.FormatValue(value, format: null, CultureInfo.InvariantCulture);

        Assert.Equal("1234", actual);
    }

    [Theory]
    [InlineData(100.0f, "N0", "100")]
    [InlineData(-1234.5f, "0.0", "-1234.5")]
    [InlineData(1234.56f, "0.00", "1234.56")]
    [InlineData(0.0f, "N0", "0")]
    public void FormatValue_Float_RoundTrip(float value, string format, string expected)
    {
        var invariant = CultureInfo.InvariantCulture;

        var formatted = BindConverter.FormatValue(value, format, invariant);

        Assert.Equal(expected, formatted);
        Assert.Equal(value.ToString(format, invariant), formatted);
    }

    [Theory]
    [InlineData(0.5, "0.00", "0.50")]
    [InlineData(100.0, "C0", "¤100")]
    [InlineData(-1234.567, "0.###", "-1234.567")]
    [InlineData(0.0, "N0", "0")]
    public void FormatValue_Double_RoundTrip(double value, string format, string expected)
    {
        var invariant = CultureInfo.InvariantCulture;

        var formatted = BindConverter.FormatValue(value, format, invariant);

        Assert.Equal(expected, formatted);
        Assert.Equal(value.ToString(format, invariant), formatted);
    }

    [Theory]
    [InlineData(100, "N0", "100")]
    [InlineData(-1234, "N0", "-1,234")]
    [InlineData(1234567, "N0", "1,234,567")]
    [InlineData(int.MaxValue, "N0", "2,147,483,647")]
    [InlineData(int.MinValue, "N0", "-2,147,483,648")]
    [InlineData(42, "D5", "00042")]
    [InlineData(255, "X", "FF")]
    public void FormatValue_Int_RoundTrip(double value, string format, string expected)
    {
        var intValue = (int)value;
        var invariant = CultureInfo.InvariantCulture;

        var formatted = BindConverter.FormatValue(intValue, format, invariant);

        Assert.Equal(expected, formatted);
        Assert.Equal(intValue.ToString(format, invariant), formatted);
    }

    [Theory]
    [InlineData(1234, "N0", "1,234")]
    [InlineData(0, "D5", "00000")]
    [InlineData(-1, "D3", "-001")]
    [InlineData(42, "X", "2A")]
    public void FormatValue_Short_RoundTrip(double value, string format, string expected)
    {
        var shortValue = (short)value;
        var invariant = CultureInfo.InvariantCulture;

        var formatted = BindConverter.FormatValue(shortValue, format, invariant);

        Assert.Equal(expected, formatted);
        Assert.Equal(shortValue.ToString(format, invariant), formatted);
    }

    [Theory]
    [InlineData(0L, "N0", "0")]
    [InlineData(1234567890L, "N0", "1,234,567,890")]
    [InlineData(long.MaxValue, "N0", "9,223,372,036,854,775,807")]
    [InlineData(long.MinValue, "N0", "-9,223,372,036,854,775,808")]
    public void FormatValue_Long_RoundTrip(double value, string format, string expected)
    {
        var longValue = (long)value;
        var invariant = CultureInfo.InvariantCulture;

        var formatted = BindConverter.FormatValue(longValue, format, invariant);

        Assert.Equal(expected, formatted);
        Assert.Equal(longValue.ToString(format, invariant), formatted);
    }

    [Fact]
    public void FormatValue_Decimal_WithFormat_DoNotMutateValue()
    {
        var value = 20129.99m;
        var beforeFormat = value;
        var invariant = CultureInfo.InvariantCulture;

        _ = BindConverter.FormatValue(value, "N2", invariant);

        Assert.Equal(beforeFormat, value);
    }

    [Fact]
    public void FormatValue_NullableDecimal_WithFormat_DoNotMutateValue()
    {
        decimal? value = 20129.99m;
        var beforeFormat = value;
        var invariant = CultureInfo.InvariantCulture;

        _ = BindConverter.FormatValue(value, "N2", invariant);

        Assert.Equal(beforeFormat, value);
    }

    [Fact]
    public void FormatValue_Decimal_EmptyFormat_ProducesInvariant()
    {
        var value = 123.45m;

        var actual = BindConverter.FormatValue(value, string.Empty, CultureInfo.InvariantCulture);

        Assert.Equal("123.45", actual);
    }

    private enum SomeLetters
    {
        A,
        B,
        C,
        Q,
    }

    [TypeConverter(typeof(PersonConverter))]
    private class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }

    private class PersonConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string text)
            {
                return JsonSerializer.Deserialize<Person>(text);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return JsonSerializer.Serialize((Person)value);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
