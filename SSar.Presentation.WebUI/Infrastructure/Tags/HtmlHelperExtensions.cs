﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HtmlTags;
using HtmlTags.Conventions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

// Credit: This code is from Jimmy Bogard's Contoso University with Razor Example
// See: https://jimmybogard.com/migrating-contoso-university-example-to-razor-pages/

namespace SSar.Presentation.WebUI.Infrastructure.Tags
{
    public static class HtmlHelperExtensions
    {
        public static HtmlTag DisplayLabel<T, TMember>(this IHtmlHelper<T> helper, Expression<Func<T, TMember>> expression)
            where T : class
        {
            return helper.Tag(expression, nameof(TagConventions.DisplayLabels));
        }

        public static HtmlTag DisplayLabel<T, TMember>(this IHtmlHelper<List<T>> helper, Expression<Func<T, TMember>> expression)
            where T : class
        {
            var library = helper.ViewContext.HttpContext.RequestServices.GetService<HtmlConventionLibrary>();
            var generator = ElementGenerator<T>.For(library, t => helper.ViewContext.HttpContext.RequestServices.GetService(t));
            return generator.TagFor(expression, nameof(TagConventions.DisplayLabels));
        }

        public static HtmlTag ValidationDiv(this IHtmlHelper helper)
        {
            return new HtmlTag("div")
                .Id("validationSummary")
                .AddClass("alert")
                .AddClass("alert-danger")
                .AddClass("hidden");
        }

        public static HtmlTag FormBlock<T, TMember>(this IHtmlHelper<T> helper,
            Expression<Func<T, TMember>> expression,
            Action<HtmlTag> labelModifier = null,
            Action<HtmlTag> inputModifier = null
        ) where T : class
        {
            labelModifier = labelModifier ?? (_ => { });
            inputModifier = inputModifier ?? (_ => { });

            var divTag = new HtmlTag("div");
            divTag.AddClass("form-group");

            var labelTag = helper.Label(expression);
            labelModifier(labelTag);

            var inputTag = helper.Input(expression);
            inputModifier(inputTag);

            divTag.Append(labelTag);
            divTag.Append(inputTag);

            return divTag;
        }
    }
}