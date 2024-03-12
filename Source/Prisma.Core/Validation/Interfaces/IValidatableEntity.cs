/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

using FluentValidation.Results;

namespace Prisma.Core.Validation;

/// <summary>
/// Represents an entity that can be validated using FluentValidation.
/// </summary>
/// <remarks>
/// The <c>IValidatableEntity</c> interface defines a contract for entities that can be validated using the FluentValidation library.
/// Implementing classes should provide logic for validating the state of the entity and return the validation results as a
/// <see cref="FluentValidation.Results.ValidationResult"/>.
/// </remarks>
public interface IValidatableEntity
{
    /// <summary>
    /// Validates the current state of the entity.
    /// </summary>
    /// <returns>
    /// A <see cref="FluentValidation.Results.ValidationResult"/> containing the results of the validation.
    /// If the validation is successful, the result should be <see cref="FluentValidation.Results.ValidationResult.IsValid"/>.
    /// </returns>
    /// <remarks>
    /// The <c>Validate</c> method should be implemented to perform entity-specific validation using FluentValidation rules.
    /// It returns a <see cref="FluentValidation.Results.ValidationResult"/> object containing information about the validation
    /// results, including any validation failures and error messages.
    /// </remarks>
    ValidationResult Validate();
}