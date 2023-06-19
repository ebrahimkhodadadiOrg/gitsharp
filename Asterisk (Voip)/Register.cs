using System;
using System.Linq;
using Rangarang_Offset.DataModel;
using System.IO;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsterNET.Manager;

namespace Rangarang_Offset
{
    public class clsVoip
    {
        #region properites

        //check voip status
        public clsGlobal.VoipStatus status;

        //Register for AMI connection
        private static ManagerConnection manager;

        //Permissions
        public static bool AllowMoeenDetail,
                    AllowViewReport,
                    AllowTicket,
                    AllowNewPerson,
                    AllowEditPerson;

        //Get List of number the user can handle
        private List<string> voipListNumber = new List<string>();

        //Get AMI user Detail
        private string voipIp, userName, userPassword;

        #endregion

        #region constructor

        public clsVoip(DataModel.tblPersonal voipUser, string voipIp, string userName, string userPassword)
        {
            this.voipListNumber.AddRange(voipUser.tblTellPersonal.Where(a => !string.IsNullOrWhiteSpace(a.Tell)).Select(a => a.Tell).Distinct().ToList());
            this.voipIp = voipIp;
            this.userName = userName;
            this.userPassword = userPassword;
        }

        #endregion

        #region functions
        public void StartVoipListening()
        {
            try
            {
                //stop the voip Listner
                StopVoipListening();

                //set Permissions
                #region Permissions
                AllowMoeenDetail = clsGlobal.context.tblPermissions.Any(a => a.GroupID == clsGlobal.GroupID && a.tblForm.Url == "frmMoinHesabDetail");
                AllowViewReport = clsGlobal.context.tblPermissions.Any(a => a.GroupID == clsGlobal.GroupID && a.tblForm.Url == "frmViewReport");
                AllowTicket = clsGlobal.context.tblPermissions.Any(a => a.GroupID == clsGlobal.GroupID && a.tblForm.Url == "frmViewTicket");
                AllowNewPerson = clsGlobal.context.sptblPermission_Forms_Select(clsGlobal.GroupID, "frmviewPersonal").ToList().Any(a => a.Url == "btnNew");
                AllowEditPerson = clsGlobal.context.sptblPermission_Forms_Select(clsGlobal.GroupID, "frmviewPersonal").ToList().Any(a => a.Url == "btnEdit");
                #endregion

                //try to connect AMI
                manager = new ManagerConnection(voipIp, 5038, userName, userPassword);
                manager.FireAllEvents = true;
                manager.PingInterval = 0;

                //when Dial Start event
                manager.Dial += Manager_Dial;

                //when Dial Hangup (زمانی که شخص تماس رو قطع میکنه)
                //manager.Hangup += Manager_Hangup;

                //All events start from here
                //manager.NewState += NewEvent;

                try
                {
                    manager.Login();
                    status = clsGlobal.VoipStatus.Connected;
                }
                catch (Exception ex)
                {
                    clsGlobal.ErrorHandling(ex, "Voip.cs - manager.Login");
                    status = clsGlobal.VoipStatus.Diconnected;
                }

            }
            catch (Exception ex)
            {
                clsGlobal.ErrorHandling(ex, "Voip.cs - StartVoipListening");
                status = clsGlobal.VoipStatus.Diconnected;
            }
        }

        public void StopVoipListening()
        {
            manager = null;
            status = clsGlobal.VoipStatus.Diconnected;
        }
        #endregion

        #region events
        private void Manager_Dial(object sender, AsterNET.Manager.Event.DialEvent e)
        {
            try
            {
                //if CallerID wan incorrect
                if (string.IsNullOrWhiteSpace(e.DialString) || string.IsNullOrWhiteSpace(e.CallerIdNum))
                    return;

                if (voipListNumber.Any(a => a == e.DialString))
                {
                    //get Text (name and number of person)
                    int? personalId;
                    string getContent = clsGlobal.MainForm.GetNumberName(e.CallerIdNum, out personalId, out string personalName);

                    //show notifactions
                    clsGlobal.MainForm.ShowCallMessage(getContent, personalId, e.CallerIdNum, personalName);
                }
            }
            catch (Exception ex)
            {
                clsGlobal.ErrorHandling(ex, "Voip.cs - Manager_Dial");
            }

        }

        //private void Manager_Hangup(object sender, AsterNET.Manager.Event.HangupEvent e)
        //{
        //    //if CallerID is incorrect
        //    if (string.IsNullOrWhiteSpace(e.CallerIdNum))
        //        return;

        //    string callerId = clsGlobal.MainForm.ReturnCleanPhoneNumber(e.CallerIdNum);

        //    if (listActiveNumbers.Any(a => a.Contains(callerId)))
        //        listActiveNumbers.Remove(callerId);
        //}

        #endregion

    }
}
