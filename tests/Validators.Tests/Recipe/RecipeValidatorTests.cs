using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe.Register;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;
using Shouldly;
using System.Diagnostics.CodeAnalysis;

namespace Validators.Tests.Recipe;

public class RecipeValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("               ")]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Intention")]
    public void Validate_ShouldHaveError_When_TitleIsEmpty(string title)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.Title = title;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_TITLE_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_TitleIsTooLong()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.Title = new string('a', 251);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_TITLE_MAX_LENGTH));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_CookTimeIsInvalid()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.CookTime = (CookTime)1000;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_COOK_TIME_INVALID));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_ThereIsNoDishType()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.DishTypes = [];

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_AT_LEAST_ONE_DISH_TYPE));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_DishTypeIsInvalid()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.DishTypes = [(DishTypes)1000];

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_DISH_TYPES_INVALID));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_ThereIsNoIngredient()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.Ingredients = [];

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_AT_LEAST_ONE_INGREDIENT));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("               ")]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters", Justification = "Intention")]
    public void Validate_ShouldHaveError_When_IngredientIsEmpty(string ingredient)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.Ingredients = [ingredient];

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INGREDIENT_EMPTY));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_IngredientIsTooLong()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.Ingredients = [new string('a', 251)];

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INGREDIENT_MAX_LENGTH));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_ThereNoInstruction()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.Instructions = [];

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_AT_LEAST_ONE_INSTRUCTION));
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    public void Validate_ShouldHaveError_When_InstructionOrderIsInvalid(int order)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.Instructions = [new RequestRecipeInstructionJson { Order = order, Description = "Invalid description" }];

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INSTRUCTION_ORDER_INVALID));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_InstructionDescriptionIsTooLong()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.Instructions = [new RequestRecipeInstructionJson { Order = 1, Description = new string('a', 2001) }];

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_MAX_LENGTH));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_When_InstructionHaveDuplicatedOrder()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeJsonBuilder.Build();

        request.Instructions = [
            new RequestRecipeInstructionJson { Order = 1, Description = "First instruction"},
            new RequestRecipeInstructionJson { Order = 1, Description = "Second instruction" },
        ];
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();

        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INSTRUCTION_ORDER_DUPLICATED));
        });
    }
}
