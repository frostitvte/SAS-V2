<%@ Page Language="VB" MasterPageFile="~/MasterPage3.master" AutoEventWireup="false" CodeFile="RptDailyReport.aspx.vb" Inherits="RptDailyReport" Title="Welcome To SAS" %>

<asp:content ID="Content2" contentplaceholderid="head" runat="server">
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <script src="../Scripts/popcalendar.js" type="text/javascript"></script>
    <link href="../style.css" rel="stylesheet" />
</asp:content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        function dateString() {
            var date, text = " "
            date = new Date()

            text += (date.getMonth() + 1) + "/"
            text += date.getDate() + "/"
            if (navigator.appName == "Netscape") {
                text += date.getYear() + 1900
            }
            if (navigator.appName == "Microsoft Internet Explorer") {
                text += date.getYear()
            }
            return (text)
        }


        function CheckFromDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtFrom.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtFrom.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtFrom.ClientID%>").value = "";
                    document.getElementById("<%=txtFrom.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function CheckToDate() {
            var digits = "0123456789/";
            var temp;
            for (var i = 0; i < document.getElementById("<%=txtToDate.ClientID %>").value.length; i++) {
                temp = document.getElementById("<%=txtToDate.ClientID%>").value.substring(i, i + 1);
                if (digits.indexOf(temp) == -1) {
                    alert("Enter Valid Date (dd/mm/yyyy)");
                    document.getElementById("<%=txtToDate.ClientID%>").value = "";
                    document.getElementById("<%=txtToDate.ClientID%>").focus();
                    return false;
                }
            }
            return true;
        }
        function getibtnFDate() {
            popUpCalendar(document.getElementById("<%=ibtnFdate.ClientID%>"), document.getElementById("<%=txtFrom.ClientID%>"), 'dd/mm/yyyy')

        }
        function getDateto() {
            popUpCalendar(document.getElementById("<%=ibtnTodate.ClientID%>"), document.getElementById("<%=txtToDate.ClientID%>"), 'dd/mm/yyyy')

        }
        function dateString() {
            var date, text = " "
            date = new Date()

            text += (date.getMonth() + 1) + "/"
            text += date.getDate() + "/"
            if (navigator.appName == "Netscape") {
                text += date.getYear() + 1900
            }
            if (navigator.appName == "Microsoft Internet Explorer") {
                text += date.getYear()
            }
            return (text)
        }

        function getDate() {
            var curenttempdate = dateString()

            var fromdate = new Date(document.getElementById("<%=txtFrom.ClientID%>").value)
            var todate = new Date(document.getElementById("<%=txtTodate.ClientID%>").value)
            var currentdate = new Date(curenttempdate)

            //Checking correct date format
            var len = document.getElementById("<%=txtFrom.ClientID%>").value
            var RegExPattern = /^((((0?[1-9]|[12]\d|3[01])[\.\-\/](0?[13578]|1[02])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|[12]\d|30)[\.\-\/](0?[13456789]|1[012])[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|((0?[1-9]|1\d|2[0-8])[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?\d{2}))|(29[\.\-\/]0?2[\.\-\/]((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00)))|(((0[1-9]|[12]\d|3[01])(0[13578]|1[02])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|[12]\d|30)(0[13456789]|1[012])((1[6-9]|[2-9]\d)?\d{2}))|((0[1-9]|1\d|2[0-8])02((1[6-9]|[2-9]\d)?\d{2}))|(2902((1[6-9]|[2-9]\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00))))$/;
            var errorMessage = 'Enter Valid Date (dd/mm/yyyy)';
            if (document.getElementById("<%=txtFrom.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtFrom.ClientID%>").value = "";
                    document.getElementById("<%=txtFrom.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtFrom.ClientID%>").value = "";
                document.getElementById("<%=txtFrom.ClientID%>").focus();
                return false;
            }


            if (document.getElementById("<%=txtTodate.ClientID%>").value.match(RegExPattern)) {
                if (len.length == 8) {
                    alert(errorMessage);
                    document.getElementById("<%=txtTodate.ClientID%>").value = "";
                    document.getElementById("<%=txtTodate.ClientID%>").focus();
                    return false;
                }
            }
            else {
                alert(errorMessage);
                document.getElementById("<%=txtTodate.ClientID%>").value = "";
                document.getElementById("<%=txtTodate.ClientID%>").focus();
                return false;
            }

            //Checking date between from and to
            var str1 = document.getElementById("<%=txtFrom.ClientID %>").value;
            var str2 = document.getElementById("<%=txtTodate.ClientID %>").value;
            var dt1 = parseInt(str1.substring(0, 2), 10);
            var mon1 = parseInt(str1.substring(3, 5), 10);
            var yr1 = parseInt(str1.substring(6, 10), 10);
            var dt2 = parseInt(str2.substring(0, 2), 10);
            var mon2 = parseInt(str2.substring(3, 5), 10);
            var yr2 = parseInt(str2.substring(6, 10), 10);
            var date1 = new Date(yr1, mon1, dt1);
            var date2 = new Date(yr2, mon2, dt2);

            if (document.getElementById("<%=txtFrom.ClientID%>").value.length != 10) {
                alert("Enter Valid Date (dd/mm/yyyy)")
                document.getElementById("<%=txtFrom.ClientID%>").focus();
                return false
            }

            if (document.getElementById("<%=txtTodate.ClientID%>").value.length != 10) {
                alert("Enter Valid Date (dd/mm/yyyy)")
                document.getElementById("<%=txtTodate.ClientID%>").focus()
                return false
            }

            if (date2 < date1) {
                alert("From Date Cannot Be Greater Than To Date");
                return false;
            }
        }

        

    </script>

    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <table style="background-image: url(../images/Sample.png);">
            <tr>
                <td style="width: 4px; height: 14px">
                </td>
                <td style="width: 14px; height: 14px">
                </td>
                <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="height: 14px">
                                <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="~/images/add.png" ToolTip="New" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label11" runat="server" Text="New"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Eddited by Zoya @26/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="~/images/save.png" ToolTip="Save" Visible="False"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label14" runat="server" Text="Save" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="height: 14px; width: 3%;">
                                <asp:ImageButton ID="ibtnDelete" runat="server" ImageUrl="~/images/delete.png" Visible="False"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label13" runat="server" Text="Delete" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnView" runat="server" ImageUrl="~/images/find.png" ToolTip="View" Visible="False"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label16" runat="server" Text="Search" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Done Eddited by Zoya @26/02/2016--%>

               <%--Commented by Zoya @26/02/2016--%>
               <%-- <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                         <tr>
                            <td style="width: 3%; height: 14px">
                                <div id="wrap">
                                    <ul id="navbar">
                                        <li><a href="#">
                                            <img id="ibtnPrint" src="/images/print.png"  width="24" height="24"  border="0" align="middle" />&nbsp;Print                                   
                                             <img src="/images/down.png" width="16" height="16" align="middle" border="0" /></a>
                                            <ul>
                                                <li><a href="#">
                                                    <asp:ImageButton ID="ibtnBMReport" runat="server" ImageUrl="~/images/print-bm.png"/>
                                                    </a></li>
                                                <li><a href="#">
                                                    <asp:ImageButton ID="ibtnEnReport" runat="server" ImageUrl="~/images/print-eng.png"/>
                                                    </a></li>
                                            </ul>
                                        </li>
                                    </ul>
                                </div>
                            </td>

                        </tr>
                    </table>
                </td>--%>
                <%--End Commented by Zoya @26/02/2016--%>

                 <%--Added by Zoya @26/02/2016 -  link to Malay Report--%>
                 <td>
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="~/images/print.png" ToolTip="Print" Height="24px"/>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Print"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%--Done Added by Zoya @26/02/2016 - link to Malay Report--%>

                <%-- Editted By Zoya @25/02/2016--%>
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnPosting" runat="server" ImageUrl="~/images/gposting.png"
                                    ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                &nbsp;<asp:Label ID="Label6" runat="server" Text="Posting" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                
                <td style="float: left;display:None; visibility:collapse;">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnOthers" runat="server" ImageUrl="~/images/gothers.png" ToolTip="Cancel" Visible="false"/>
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label5" runat="server" Text="Others" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="float: left">
                    <table style="border-collapse: collapse;" class="menuoff" onmouseover="className='menuon';"
                        onmouseout="className='menuoff';">
                        <tr>
                            <td style="width: 3%; height: 14px">
                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="~/images/cancel.png" ToolTip="Cancel"
                                    OnClick="ibtnCancel_Click1" />
                            </td>
                            <td style="width: 3%; height: 14px">
                                <asp:Label ID="Label18" runat="server" Text="Cancel"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <%-- Done Editted By Zoya @25/02/2016--%>
                <%--Editted By Zoya @2/03/2016--%>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnFirst" runat="server" ImageUrl="~/images/new_last.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnPrevs" runat="server" ImageUrl="~/images/new_prev.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:TextBox ID="txtRecNo" runat="server" ReadOnly="true" CssClass="text_box" Width="52px"
                        AutoPostBack="True" Style="text-align: right" disabled="disabled" TabIndex="1"
                        dir="ltr" Visible="false"></asp:TextBox>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:Label ID="Label47" runat="server" Visible="false">Of</asp:Label>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:Label ID="lblCount" runat="server" Width="20px" Visible="false"></asp:Label>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="~/images/new_next.png" Visible="false"/>
                </td>
                <td style="float: left;display:None; visibility:collapse;width: 2%; height: 14px">
                    <asp:ImageButton ID="ibtnLast" runat="server" ImageUrl="~/images/new_first.png" Visible="false"/>
                </td>
                 <%--Done Editted By Zoya @2/03/2016--%>
                <td style="width: 2%; height: 14px">
                </td>
                <td style="width: 100%; height: 14px">
                </td>
                <td style="width: 100%; height: 14px">
                </td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%;">
            <tr>
                <td class="vline" style="width: 98%; height: 1px">
                </td>
            </tr>
        </table>
        <table class="mainbg" style="width: 100%;">
            <tr>
                <td style="width: 494px; height: 39px;">
                    <asp:SiteMapPath ID="SiteMapPath2" runat="server" Width="260px">
                    </asp:SiteMapPath>
                </td>
                <td align="right" class="pagetext" style="height: 39px">
                    <asp:Label ID="lblMenuName" runat="server" Width="404px"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="Panel2" runat="server" Width="100%">
        <table width="100%">
            <tr>
                <td width="1%">
                </td>
                <td width="98%">
                    <fieldset style="width: 100%;border: thin solid #A6D9F4;">
                        <legend><strong><span style="color: #000000;">Selection Criteria</span></strong></legend>
                        <table>
                            <tr>
                                <td style="width: 47px; height: 16px">
                                    <asp:Label ID="Label1" runat="server" Text="From"></asp:Label>
                                </td>
                                <td style="height: 16px">
                                    <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 10px; height: 16px">
                                    <asp:Image ID="ibtnFDate" runat="server" ImageUrl="~/images/cal.gif" />
                                </td>
                                <td style="width: 9px; height: 16px">
                                    <asp:Label ID="Label2" runat="server" Text="To"></asp:Label>
                                </td>
                                <td style="width: 63px; height: 16px">
                                    <asp:TextBox ID="txtTodate" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 648px; height: 16px">
                                    <asp:Image ID="ibtnTodate" runat="server" ImageUrl="~/images/cal.gif" />
                                </td>
                                <td style="width: 1px; height: 16px">
                                </td>
                                <td style="width: 1px; height: 16px">
                                </td>
                                <td style="width: 1px; height: 16px">
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td style="height: 16px; width: 44px;">
                                    <asp:Label ID="Label3" runat="server" Text="Sort By"></asp:Label>
                                </td>
                                <td style="height: 16px">
                                    <asp:DropDownList ID="ddlStuStatus" runat="server" Width="135px"
                                        OnSelectedIndexChanged="ddlStuStatus_SelectedIndexChanged">
                                        <asp:ListItem Value="-1" Selected="True">-- Select --</asp:ListItem>
                                        <asp:ListItem Value="MatricNo">Matric No.</asp:ListItem>
                                        <asp:ListItem>Status</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td style="width: 113px; height: 16px">
                </td>
            </tr>
            <tr>
                <td style="height: 16px; width: 3px;">
                </td>
                <td style="width: 1005px; height: 16px">
                    &nbsp;
                </td>
                <td style="height: 16px;">
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
