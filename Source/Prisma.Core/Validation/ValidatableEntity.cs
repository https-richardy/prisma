/*
    author: Richard Garcia (https://github.com/https-richardy)
    license: Licensed under the MIT License
*/

using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;

namespace Prisma.Core.Validation;

/// <summary>
/// Represents a base class for entities that require self-validation using FluentValidation.
/// </summary>
/// <typeparam name="T">The type of the derived entity.</typeparam>
/// <remarks>
/// The <see cref="ValidatableEntity{T}"/> class serves as a base class for entities that require validation using the FluentValidation library.
/// In scenarios where entities are responsible for validating their own state, rather than delegating the definition of validation rules to other classes,
/// this base class allows derived entities to define and execute their own validation logic internally.
/// </remarks>
public abstract class ValidatableEntity<T> : IValidatableEntity where T: ValidatableEntity<T>
{
    /// <summary>
    /// The validator instance used for validating the entity.
    /// </summary>
    protected AbstractValidator<T> Validator;

    /// <summary>
    /// Gets a value indicating whether the entity is valid according to its validation rules.
    /// </summary>
    /// <remarks>
    /// The <c>IsValid</c> property returns <c>true</c> if all validation rules pass successfully and the entity's state conforms to the defined rules.
    /// Otherwise, it returns <c>false</c>.
    /// </remarks>
    public bool IsValid => Validate().IsValid;

    /// <summary>
    /// Initializes a new instance of the <c>ValidatableEntity</c> class.
    /// </summary>
    protected ValidatableEntity()
    {
        Validator = new InlineValidator<T>();
    }

    /// <summary>
    /// Validates the current state of the entity.
    /// </summary>
    /// <returns>A <see cref="ValidationResult"/> containing the results of the validation.</returns>
    /// <remarks>
    /// The <c>Validate</c> method validates the current state of the entity using the configured validator instance.
    /// It returns a <see cref="ValidationResult"/> object containing the results of the validation, including any validation errors.
    /// </remarks>
    public ValidationResult Validate()
    {
        return Validator.Validate((T)this);
    }

    /// <summary>
    /// Adds a validation rule for a specified property of the entity.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="expression">An expression representing the property to validate.</param>
    /// <returns>An <see cref="IRuleBuilderInitial{T,TProperty}"/> object that can be used to further configure the validation rule.</returns>
    /// <remarks>
    /// The <c>AddRule</c> method adds a validation rule for the specified property of the entity.
    /// It returns an <see cref="IRuleBuilderInitial{T,TProperty}"/> object that allows further configuration of the validation rule.
    /// </remarks>
    protected IRuleBuilderInitial<T, TProperty> AddRule<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        return Validator.RuleFor(expression);
    }
}
