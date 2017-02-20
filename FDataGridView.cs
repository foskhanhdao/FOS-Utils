using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace FOS_Utils
{
    public class FDataGridViewOverride : DataGridView
    {
        
        IDataGridViewKeyProcess keyProcess = null;
       

       
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //cell is not in Edit mode   
            if (keyData == Keys.Enter || keyData == Keys.Tab)
            {
                DoEnterKeyPressed();
                return true;
            }
            if (keyData == Keys.Up && this.EditingControl != null
                && !(this.EditingControl is ComboBoxEditingControl)
                && !(this.EditingControl is DataGridViewComboBoxEditingControl))
            {
                DoUpKeyPressed();
                return true;
            }
            if (keyData == Keys.Down && this.EditingControl != null
                && !(this.EditingControl is ComboBoxEditingControl)
                && !(this.EditingControl is DataGridViewComboBoxEditingControl))
            {
                DoDownKeyPressed();
                return true;
            }
            if (keyData == (Keys.Tab | Keys.Shift))
            {
                DoShiftTabKeyPressed();
                return true;
            }
            if (keyData == Keys.F1 || keyData == Keys.F2
                || keyData == Keys.F3 || keyData == Keys.F4
                || keyData == Keys.F5 || keyData == Keys.F6
                || keyData == Keys.F7 || keyData == Keys.F8
                || keyData == Keys.F9 || keyData == Keys.F10
                || keyData == Keys.F11 || keyData == Keys.F12
                || keyData == Keys.Escape )
            {
                DoFunctionKeyPressed(keyData);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        

     
        public void InitKeyProcess(IDataGridViewKeyProcess iDataGridViewKeyProcess)
        {
            this.keyProcess = iDataGridViewKeyProcess;
        }
        private void DoEnterKeyPressed()
        {
            if (this.keyProcess != null)
                this.keyProcess.OnEnterKeyProcess();
        }
        private void DoUpKeyPressed()
        {
            if (this.keyProcess != null)
                this.keyProcess.OnUpKeyProcess();
        }
        private void DoDownKeyPressed()
        {
            if (this.keyProcess != null)
                this.keyProcess.OnDownKeyProcess();
        }
        private void DoShiftTabKeyPressed()
        {
            if (this.keyProcess != null)
                this.keyProcess.OnShiftTabKeyProcess();
        }

        private void DoFunctionKeyPressed(Keys  key)
        {
            if (this.keyProcess != null)
                this.keyProcess.OnFunctionKeyProcess(key);
        }
           
    }
}

