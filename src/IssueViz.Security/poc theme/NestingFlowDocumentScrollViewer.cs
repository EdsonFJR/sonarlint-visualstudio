using System.Windows.Controls;
using System.Windows.Input;

namespace SonarLint.VisualStudio.IssueVisualization.Security.Taint.TaintList.poc_theme
{
    public class NestingFlowDocumentScrollViewer : FlowDocumentScrollViewer
    {
        public override void BeginInit()
        {
            base.BeginInit();

            this.AddHandler(MouseWheelEvent, (MouseWheelEventHandler)OnMouseWheelEvent, handledEventsToo: true);
        }

        private void OnMouseWheelEvent(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled && e.OriginalSource != this )
            {
                e.Handled = false;
                OnMouseWheel(e);
            }
        }

        // This approach also works
        //protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        //{
        //    base.OnPreviewMouseWheel(e);
        //    //if (e.OriginalSource != this)
        //    //{
        //    //    this.OnMouseWheel(e);
        //    //}
        //}
    }
}
