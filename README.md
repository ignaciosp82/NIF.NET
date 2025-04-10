# NIF.NET
A validator for spanish NIFs

## Usage
```csharp
// Validate any type of NIF
INIFValidator nifValidator = new NIFValidator();

// Validate NIF of natural person (DNI)
bool isValidDni = nifValidator.IsValid("12345678Z", NIFType.NaturalPerson);

// Validate NIF of legal person (old CIF)
bool isValidCif = nifValidator.IsValid("N7685328B", NIFType.LegalPerson);

// Validate NIF of foreigner person (NIE)
bool isValidNie = nifValidator.IsValid("X1234567L", NIFType.Foreigner);