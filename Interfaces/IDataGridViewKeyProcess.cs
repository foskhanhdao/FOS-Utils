
using System.Windows.Forms;

namespace FOS_Utils
{
    public interface IDataGridViewKeyProcess
    {
        void OnEnterKeyProcess();
        void OnUpKeyProcess();
        void OnDownKeyProcess();
        void OnShiftTabKeyProcess();
        void OnFunctionKeyProcess(Keys  key);
    }
}
