﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;

namespace CMPSC487W_Project2.Entities.Forms.Inputs
{
    public interface IRenderable
    {
        public IHtmlContent Render(IHtmlHelper Html, string outerClass, string outerStyle);
    }
    public interface IRenderable<T>
    {
        public IHtmlContent Render(IHtmlHelper<T> Html, T Model, string outerClass, string outerStyle);
    }


    public abstract class Input<T> : IRenderable<T>
    {
        public Input(string label)
        {
            Label = label;
        }


        protected abstract void RenderInner(IHtmlHelper<T> Html, T Model, HtmlConcat htmlConcat,
            string outerClass, string outerStyle);
        public abstract Expression GetField();
        //public abstract Type GetFieldType();
        //public abstract string GetName();
        /*public string GetName(IHtmlHelper<T> Html)
        {
            return Html.NameFor((Expression<Func<T, TResult>>)GetField());
        }*/
        /*protected string GetName()
        {
            Expression<Func<T, TResult>> Field = (Expression<Func<T, TResult>>)GetField();
            try
            {
                MemberExpression expression = (MemberExpression)Field.Body;
                string name = expression.Member.Name;
                while (expression.Expression.GetType().IsSubclassOf(typeof(MemberExpression)))//.Member.Name != Field.Parameters[0].Name)
                {
                    expression = (MemberExpression)expression.Expression;
                    name = $"{expression.Member.Name}.{name}";
                }
                return name;
            }
            catch (InvalidCastException)
            {
                try
                {
                    return ((MemberExpression)((UnaryExpression)Field.Body).Operand).Member.Name;
                }
                catch (InvalidCastException)
                {
                    return null;
                }
            }
        }*/
        //public abstract string GetValue(T obj);
        /*protected string GetValue(T obj)
        {
            
            Expression<Func<T, TResult>> Field = (Expression<Func<T, TResult>>)GetField();
            TResult result = Field.Compile().Invoke(obj);
            return result?.ToString();
        }*/
        /*public string GetValue<TResult>(IHtmlHelper<T> Html)
        {
            return Html.ValueFor((Expression<Func<T, TResult>>)GetField());
        }*/
        public string GetValue(IHtmlHelper<T> Html)
        {
            //var f = (Expression<Func<T,T>>)GetField();
            return Html.Value(GetField().ToString());
            //return Html.ValueFor(f);
            //return Html.ValueFor((Expression<Func<T,dynamic>>)GetField());
        }
        //public string GetName(IHtmlHelper<T> Html)
        public string GetName()
        {
            //var a = NameAndIdProvider.GetFullHtmlFieldName(Html.ViewContext, GetField().ToString());
            //return Html.NameFor(GetField());
            dynamic b = GetField();
            string body = b.Body.ToString();
            return body.ToString()[(body.ToString().IndexOf(".") + 1)..].Replace(".", "_");
            //return Html.Name(GetField().ToString());
        }


        protected bool Required { get; set; }
        protected string Label { get; set; }
        protected string OuterClass { get; set; }
        protected string PlaceHolder { get; set; }
        public string OnChange { get; set; }
        public void SetOnChange(string divSelector)
        {
            OnChange = $"changeHiddenDiv({divSelector},this)";
        }
        public IHtmlContent Render(IHtmlHelper<T> Html, T Model, string outerClass, string outerStyle)
        {
            HtmlConcat htmlConcat = new();
            RenderInner(Html, Model, htmlConcat, outerClass, outerStyle);
            return htmlConcat.Concatenate();
        }


        protected string ToJSString(bool b) { return b.ToString().ToLower(); }
    }
}