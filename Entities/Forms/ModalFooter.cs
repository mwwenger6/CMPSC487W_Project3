
using CMPSC487W_Project2.Entities.Forms.Elements;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMPSC487W_Project2.Entities.Forms
{
    public class ModalFooter
    {
        /// <summary>
        /// Creates a new instance of a modal footer with button and a note
        /// </summary>
        /// <param name="submitBtnText">text for submit button</param>
        /// <param name="objectName">name of object represented by modal</param>
        /// <param name="modalType">type of modal, for submit action</param>

        public ModalFooter(string submitBtnText, string objectName, string modalType)
        {
            string submitColor = "success";
            Buttons = new()
            {
                new Button("Close", id:null, "secondary", isDismiss:true),
                new Button(submitBtnText, $"{modalType}{objectName}ModalSubmit", submitColor){
                    OnClick = $"{modalType}Object('{objectName}')"
                }
            };
            NoteClass = "bg-warning";
        }
        /* public ModalFooter(string objectName, string modalType)
         {
             Buttons = new()
             {
                 new Button("Close", id:null, "secondary", isDismiss:true),
                 new Button("Download", $"{modalType}{objectName}ModalSubmit", modalType==IModal.DELETE?"danger":"blue"){
                     OnClick = $"{modalType}Object('{objectName}')"
                 },
                 new Button("Send", $"{modalType}{objectName}ModalSubmit", modalType==IModal.DELETE?"danger":"blue"){
                     OnClick = $"{modalType}Object('{objectName}')"
                 }
             };
             NoteClass = "bg-warning";
         }*/
        private string Note { get; set; }
        private string NoteClass { get; set; }

        public List<Button> Buttons { get; set; }

        public ModalFooter SetButtons(List<Button> buttons)
        {
            Buttons = buttons;
            return this;
        }
        public ModalFooter SetNote(string noteText, string @class = null)
        {
            Note = noteText;
            NoteClass = @class ?? NoteClass;
            return this;
        }

        public IHtmlContent GetHtml()
        {
            HtmlConcat htmlConcat = new();
            htmlConcat.Add("<div class=\"modal-footer\">" +
                $"<span class=\"modal-footer-note {NoteClass}\">{Note}</span>");
            /*foreach (Button button in Buttons)
                htmlConcat.Add(button.Render());*/
            htmlConcat.Add(Buttons.Select(b => b.Render()).ToArray());
            htmlConcat.Add("</div>");
            return htmlConcat.Concatenate();
        }
    }
}