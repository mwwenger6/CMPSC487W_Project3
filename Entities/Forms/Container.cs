using CMPSC487W_Project3.Entities.Forms.Inputs;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace CMPSC487W_Project3.Entities.Forms
{
    public interface IContainer
    {
        public static HtmlString RENDER_TOOLTIPS()
        {
            return new HtmlString("[...$('[data-bs-toggle=\"tooltip\"]')].map(ele=>new bootstrap.Tooltip(ele));");
        }
    }

    public class Container<T> : IContainer, IRenderable<T>
    {
        public Container()
        {
            Contents = new();
        }
        public Container(string title)
        {
            Contents = new();
            Title = title;
        }

        public string Title { get; set; }
        public List<IRenderable<T>> Contents { get; set; }
        //public List<Input<T>> Inputs { get; set; }


        /// <summary>
        /// Creates a text input for a field
        /// </summary>
        /// <param name="field">property of T that the editor will modify</param>
        /// <param name="label">text displayed on the small label</param>
        /// <param name="placeHolder">placeholder text on the input</param>
        /// <param name="minLength">minLength attribute of the input</param>
        /// <param name="required">marks the input/label as required</param>
        /// <param name="toggleDiv">(optional) div to be shown, when this input has a specified value</param>
        public void AddInput(Expression<Func<T, IComparable>> field, string label, string placeHolder = "",
            bool required = true, bool readOnly = false, string outerClass = "col-12 col-md-6", bool hideable = false
        )
        {
            Contents.Add(new TextInput<T, IComparable>(field, label, placeHolder, required, readOnly, outerClass, hideable));

        }
        public void AddInput(Expression<Func<T, IComparable>> field, string label, int rows, string placeHolder = "",
            bool required = true, bool readOnly = false, string outerClass = "col-12 col-md-6", bool hideable = false
        )
        {
            Contents.Add(new MultiLineInput<T, IComparable>(field, label, rows, placeHolder, required, readOnly, outerClass, hideable));
        }
        public void AddInput<TResult>(Expression<Func<T, TResult>> field, string label, SelectList dropDownList,
            bool required = true, bool disabled = false, string outerClass = "col-12 col-md-6"
        )
        {
            Contents.Add(new DropDown<T, TResult>(field, label, required, dropDownList, disabled,
                outerClass));
        }
        public void AddPassword(Expression<Func<T, IComparable>> field, string label, string placeHolder = "",
            bool required = true, bool readOnly = false, string outerClass = "col-12 col-md-6"
        )
        {
            Contents.Add(new PasswordInput<T, IComparable>(field, label, placeHolder, required, readOnly, outerClass));

        }
        public void AddInput<TResult>(Expression<Func<T, TResult>> field, bool hidden)
        {
            Contents.Add(new HiddenInput<T, TResult>(field, true));
        }

        /// <summary>
        /// Creates a File input for a field
        /// </summary>
        /// <param name="field">property of T that the editor will modify</param>
        /// <param name="label">text displayed on the small label</param>
        /// <param name="required">marks the input/label as required</param>
        /// <param name="toggleDiv">(optional) div to be shown, when this input has a specified value</param>
        public void AddInput(Expression<Func<T, IFormFile>> field, string label, bool required = true,
            string outerClass = "col-12 col-md-6"
        )
        {
            Contents.Add(new FileInput<T>(field, label, required, outerClass));
        }

        public Input<T> GetLastInput()
        {
            /*var last = Contents.Last() as Input<T>;
            var c = Contents.Where(c =>
                !(c.GetType().IsSubclassOf(typeof(Input<T>)) || c.GetType() == typeof(Input<T>))
            ).ToList();*/
            Type baseType = typeof(Input<T>);
            IRenderable<T> last = Contents.Last(c =>
                c.GetType() == baseType || c.GetType().IsSubclassOf(baseType));
            return (Input<T>)last;
        }

        /// <summary>
        /// loops through all contents and renders them passing all parameters inward
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="Model"></param>
        /// <param name="outerClass"></param>
        /// <param name="outerStyle"></param>
        /// <returns></returns>
        public virtual IHtmlContent Render(IHtmlHelper<T> Html, T Model, string outerClass,
        //public virtual IHtmlContent Render(IHtmlHelper<T> Html, T Model, string outerClass = "col-12",
            string outerStyle = "margin-bottom:5px;")
        {
            HtmlConcat htmlConcat = new();
            foreach (IRenderable<T> content in Contents)
                htmlConcat.Add(content.Render(Html, Model, outerClass, outerStyle));
            return htmlConcat.Concatenate();
        }

        public string GetValue(T model)
        {
            throw new NotImplementedException();
        }
    }
}