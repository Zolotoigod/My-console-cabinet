using System;

namespace FileCabinetApp.Validation.Service
{
    public static class ValidatorBuilderExtensions
    {
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var validationData = ParametersCreater.GetDefaultParams();
            return builder.ValidateFirstName(validationData.MinFirstNameLength, validationData.MaxFirstNameLength)
                   .ValidateLastName(validationData.MinLastNameLength, validationData.MaxLastNameLength)
                   .ValidateDateOfBirth(validationData.DateOfBirthFrom, validationData.DateOfBirthTo)
                   .ValidateType(validationData.Types)
                   .ValidateNumber(validationData.NumberMax)
                   .ValidateBalance(validationData.BalanceMax)
                   .Create();
        }

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var validationData = ParametersCreater.GetCustomParams();
            return builder.ValidateFirstName(validationData.MinFirstNameLength, validationData.MaxFirstNameLength)
                   .ValidateLastName(validationData.MinLastNameLength, validationData.MaxLastNameLength)
                   .ValidateDateOfBirth(validationData.DateOfBirthFrom, validationData.DateOfBirthTo)
                   .ValidateType(validationData.Types)
                   .ValidateNumber(validationData.NumberMax)
                   .ValidateBalance(validationData.BalanceMax)
                   .Create();
        }

        public static IRecordValidator ReadConfig(this ValidatorBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var validationData = ParametersCreater.ReadConfigParams(Defines.ConfigPath);
            return builder.ValidateFirstName(validationData.MinFirstNameLength, validationData.MaxFirstNameLength)
                   .ValidateLastName(validationData.MinLastNameLength, validationData.MaxLastNameLength)
                   .ValidateDateOfBirth(validationData.DateOfBirthFrom, validationData.DateOfBirthTo)
                   .ValidateType(validationData.Types)
                   .ValidateNumber(validationData.NumberMax)
                   .ValidateBalance(validationData.BalanceMax)
                   .Create();
        }
    }
}
