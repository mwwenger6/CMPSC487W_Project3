using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace CMPSC487W_Project3.Entities.Forms.Inputs;

public class DropDown<T, TResult> : Input<T>
{

    public DropDown(Expression<Func<T, TResult>> field, string label, bool required, SelectList list,
        bool disabled, string outerClass
    ) : base(label)
    {
        DropDownList = list ?? throw new ArgumentNullException(nameof(list));
        Disabled = disabled;
        Field = field;
        Label = label;
        Required = required;
        OuterClass = outerClass;
    }
    //public DropDown(Expression<Func<T, TResult>> field, string label, bool required, Func<SelectList> ddFunc,
    //    bool disabled, string outerClass
    //)
    //{
    //    DropDownFunc = ddFunc;
    //    Disabled = disabled;
    //    Field = field;
    //    Label = label;
    //    Required = required;
    //    OuterClass = outerClass;
    //}
    protected Expression<Func<T, TResult>> Field { get; set; }
    private SelectList DropDownList { get; set; }
    protected SelectList GetDropDownList()
    {
        return DropDownList ?? DropDownFunc.Invoke();
    }
    private Func<SelectList> DropDownFunc { get; set; }
    protected bool Disabled { get; set; }

    public override Expression GetField()
    {
        return Field;
    }
    //public override string GetName()
    //{
    //    try
    //    {
    //        return ((MemberExpression)Field.Body).Member.Name;
    //    }
    //    catch (InvalidCastException)
    //    {
    //        return ((MemberExpression)((UnaryExpression)Field.Body).Operand).Member.Name;
    //    }
    //}

    //public override string GetValue(T obj)
    //{
    //    TResult result = Field.Compile().Invoke(obj);
    //    return result.ToString();
    //}

    protected override void RenderInner(IHtmlHelper<T> Html, T Model, HtmlConcat htmlConcat,
        string outerClass, string outerStyle
    )
    {
        object labelHhtmlAttr = new { @class = Required ? "required" : string.Empty };

        object htmlAttr = Disabled
            ? new
            {
                disabled = "disabled",
                req = ToJSString(Required)
            }
            : new { req = ToJSString(Required) };

        htmlConcat.Add($"<div class=\"myform-control {outerClass ?? OuterClass}\" style=\"{outerStyle}\">");
        htmlConcat.Add(Html.LabelFor(Field, Label, labelHhtmlAttr));
        htmlConcat.Add(Html.DropDownListFor(Field, GetDropDownList(), htmlAttr));
        htmlConcat.Add("</div>");
    }
}