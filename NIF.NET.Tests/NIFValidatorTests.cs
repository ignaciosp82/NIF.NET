namespace NIF.NET.Tests
{
    public class NIFValidatorTests
    {
        private readonly INIFValidator nifValidator;

        public NIFValidatorTests()
            => nifValidator = new NIFValidator();

        [Theory]
        [InlineData("12345678Z", true)]
        [InlineData("54904865R", true)]
        [InlineData("44041799L", true)]
        [InlineData("25375691K", true)]
        [InlineData("A87692000", true)]
        [InlineData("W0236315H", true)]
        [InlineData("R8823632H", true)]
        [InlineData("Y8400821Q", true)]
        [InlineData("Z4476811G", true)]
        [InlineData("Y9255682J", true)]
        [InlineData("1234567A", false)]
        [InlineData("12345678I", false)]
        [InlineData("B1234567", false)]
        [InlineData("K12345678", false)]
        [InlineData("X12345678", false)]
        [InlineData("Y1234567@", false)]
        [InlineData("Z1234567O", false)]
        [InlineData("M1234567P", false)]
        [InlineData("N12345678", false)]
        [InlineData("W1234567X", false)]
        [InlineData(null, false)]
        public void IsValid_ShouldReturnExpectedResult(string nif, bool expectedResult)
            => Assert.Equal(expectedResult, nifValidator.IsValid(nif));

        [Theory]
        [InlineData("12345678Z", NIFType.NaturalPerson, true)]
        [InlineData("87654321X", NIFType.NaturalPerson, true)]
        [InlineData("00000000T", NIFType.NaturalPerson, true)]
        [InlineData("99999999R", NIFType.NaturalPerson, true)]
        [InlineData("N7685328B", NIFType.LegalPerson, true)]
        [InlineData("J16439358", NIFType.LegalPerson, true)]
        [InlineData("V01090885", NIFType.LegalPerson, true)]
        [InlineData("X1234567L", NIFType.Foreigner, true)]
        [InlineData("Y1483305A", NIFType.Foreigner, true)]
        [InlineData("Z4586760J", NIFType.Foreigner, true)]
        [InlineData("12345678B", NIFType.NaturalPerson, false)]
        [InlineData("8765432X", NIFType.NaturalPerson, false)]
        [InlineData("00000000A", NIFType.NaturalPerson, false)]
        [InlineData("ABCDEFGHI", NIFType.NaturalPerson, false)]
        [InlineData(null, NIFType.NaturalPerson, false)]
        [InlineData("B12345", NIFType.LegalPerson, false)]
        [InlineData("12345678", NIFType.LegalPerson, false)]
        [InlineData("A8765432Z", NIFType.LegalPerson, false)]
        [InlineData(null, NIFType.LegalPerson, false)]
        [InlineData("X1234567P", NIFType.Foreigner, false)]
        [InlineData("Y876543Z", NIFType.Foreigner, false)]
        [InlineData("W0000000T", NIFType.Foreigner, false)]
        [InlineData(null, NIFType.Foreigner, false)]
        public void IsValid_WithType_ShouldReturnExpectedResult(string nif, NIFType type, bool expectedResult)
            => Assert.Equal(expectedResult, nifValidator.IsValid(nif, type));
    }
}