Imports System.Data
Imports System.Collections.Generic
Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.Entities

Partial Class universityProfile
    Inherits System.Web.UI.Page
    Dim CFlag As String
    Dim DFlag As String
    Dim ListObjects As List(Of UniversityProfileEn)
    Private ErrorDescription As String
    ''Private LogErrors As LogError

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Visible = False
        If Not IsPostBack() Then
            ListObjects = Nothing
            'Client Validations
            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")

            'edit Zoya 16/2/2016
            'txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            'txtPhone.Attributes.Add("onKeypress", "checkValue()")
            'txtFax.Attributes.Add("onKeypress", "checkValue()")
            txtPostalCode.Attributes.Add("onKeypress", "isNumberKey(event)")
            txtPhone.Attributes.Add("onKeypress", "isNumberKey(event)")
            txtFax.Attributes.Add("onKeypress", "isNumberKey(event)")

            'load PageName
            Menuname(CInt(Request.QueryString("Menuid")))
            LoadUserRights()
            Session("ListObj") = Nothing
            DisableRecordNavigator()
        End If
        lblMsg.Text = ""
    End Sub

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
    End Sub

    Protected Sub ibtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDelete.Click
        onDelete()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        onAdd()

    End Sub
    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecNo.TextChanged
        If Trim(txtRecNo.Text).Length = 0 Then
            txtRecNo.Text = 0
            If lblCount.Text <> Nothing Then
                If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                    txtRecNo.Text = lblCount.Text
                    'txtRecNo.Text = String.Empty
                    'txtRecNo.Enabled = False

                End If
                FillData(CInt(txtRecNo.Text) - 1)
            Else
                txtRecNo.Text = ""
                'txtRecNo.Enabled = True

            End If
        Else
            If lblCount.Text <> Nothing Then
                If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                    txtRecNo.Text = lblCount.Text
                End If
                FillData(CInt(txtRecNo.Text) - 1)
            Else
                txtRecNo.Text = ""
            End If
        End If
    End Sub

    Protected Sub ibtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click
        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                onClearData()
                If ibtnNew.Enabled = False Then
                    ibtnSave.Enabled = False
                    ibtnSave.ImageUrl = "images/gsave.png"
                    ibtnSave.ToolTip = "Access Denied"
                End If
            Else
                LoadListObjects()
            End If
        Else
            LoadListObjects()
        End If
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        onClearData()
    End Sub

    Protected Sub ibtnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNext.Click
        OnMoveNext()
    End Sub

    Protected Sub ibtnLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLast.Click
        OnMoveLast()
    End Sub

    Protected Sub ibtnPrevs_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnPrevs.Click
        OnMovePrevious()
    End Sub

    Protected Sub ibtnFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnFirst.Click
        OnMoveFirst()
    End Sub
#Region "Methods"
    ''' <summary>
    ''' Method to Enable or Disable Navigation Buttons
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisableRecordNavigator()
        Dim flag As Boolean
        If Session("ListObj") Is Nothing Then
            flag = False
            txtRecNo.Text = ""
            lblCount.Text = ""
        Else
            flag = True
        End If
        ibtnFirst.Enabled = flag
        ibtnLast.Enabled = flag
        ibtnPrevs.Enabled = flag
        ibtnNext.Enabled = flag
        If flag = False Then
            ibtnFirst.ImageUrl = "images/gnew_first.png"
            ibtnLast.ImageUrl = "images/gnew_last.png"
            ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            ibtnNext.ImageUrl = "images/gnew_next.png"
        Else
            ibtnFirst.ImageUrl = "images/new_last.png"
            ibtnLast.ImageUrl = "images/new_first.png"
            ibtnPrevs.ImageUrl = "images/new_Prev.png"
            ibtnNext.ImageUrl = "images/new_next.png"

        End If
    End Sub
    ''' <summary>
    ''' Method to Validate Before Save
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        If Trim(txtName.Text).Length = 0 Then

            txtName.Text = Trim(txtName.Text)
            lblMsg.Text = "Enter valid Descripsion "
            lblMsg.Visible = True
            txtName.Focus()
            Exit Sub
        End If
        If Trim(txtSName.Text).Length = 0 Then

            txtSName.Text = Trim(txtSName.Text)
            lblMsg.Text = "Enter valid Short Description  "
            lblMsg.Visible = True
            txtSName.Focus()
            Exit Sub
        End If
        onSave()
    End Sub
    ''' <summary>
    ''' Method to Load UserRights
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadUserRights()
        Dim obj As New UsersBAL
        Dim eobj As New UserRightsEn

        Try
            eobj = obj.GetUserRights(CInt(Request.QueryString("Menuid")), CInt(Session("UserGroup")))
        Catch ex As Exception
            LogError.Log("UniversityProfile", "LoadUserRights", ex.Message)
        End Try
        'Rights for Add

        If eobj.IsAdd = True Then
            'ibtnSave.Enabled = True
            onAdd()
            ibtnNew.ImageUrl = "images/add.png"
            ibtnNew.Enabled = True
        Else
            ibtnNew.ImageUrl = "images/gadd.png"
            ibtnNew.Enabled = False
            ibtnNew.ToolTip = "Access Denied"

        End If
        'Rights for Edit
        If eobj.IsEdit = True Then
            ibtnSave.Enabled = True
            ibtnSave.ImageUrl = "images/save.png"
            ibtnSave.ToolTip = "Edit"
            If eobj.IsAdd = False Then
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                ibtnSave.ToolTip = "Access Denied"
            End If

            Session("EditFlag") = True

        Else
            Session("EditFlag") = False
            ibtnSave.Enabled = False
            ibtnSave.ImageUrl = "images/gsave.png"
        End If
        'Rights for View
        ibtnView.Enabled = eobj.IsView
        If eobj.IsView = True Then
            ibtnView.ImageUrl = "images/find.png"
            ibtnView.Enabled = True
        Else
            ibtnView.ImageUrl = "images/gfind.png"
            ibtnView.ToolTip = "Access Denied"
        End If
        'Rights for Delete
        If eobj.IsDelete = True Then
            ibtnDelete.ImageUrl = "images/delete.png"
            ibtnDelete.Enabled = True
        Else
            ibtnDelete.ImageUrl = "images/gdelete.png"
            ibtnDelete.ToolTip = "Access Denied"
            ibtnDelete.Enabled = False
        End If
        'Rights for Print
        ibtnPrint.Enabled = eobj.IsPrint
        If eobj.IsPrint = True Then
            ibtnPrint.Enabled = True
            ibtnPrint.ImageUrl = "images/print.png"
            ibtnPrint.ToolTip = "Print"
        Else
            ibtnPrint.Enabled = False
            ibtnPrint.ImageUrl = "images/gprint.png"
            ibtnPrint.ToolTip = "Access Denied"
        End If
        If eobj.IsOthers = True Then
            ibtnOthers.Enabled = True
            ibtnOthers.ImageUrl = "images/others.png"
            ibtnOthers.ToolTip = "Others"
        Else
            ibtnOthers.Enabled = False
            ibtnOthers.ImageUrl = "images/gothers.png"
            ibtnOthers.ToolTip = "Access Denied"
        End If
        If eobj.IsPost = True Then
            ibtnPosting.Enabled = True
            ibtnPosting.ImageUrl = "images/posting.png"
            ibtnPosting.ToolTip = "Posting"
        Else
            ibtnPosting.Enabled = False
            ibtnPosting.ImageUrl = "images/gposting.png"
            ibtnPosting.ToolTip = "Access Denied"
        End If
    End Sub
    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onAdd()
        Session("PageMode") = "Add"
        ibtnSave.Enabled = True
        ibtnSave.ImageUrl = "images/save.png"
        PnlAdd.Visible = True
        onClearData()

    End Sub
    ''' <summary>
    ''' Method to Save and Update University Profile
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onSave()

        Dim bsobj As New UniversityProfileBAL
        Dim eobj As New UniversityProfileEn

        Dim RecAff As Integer
        eobj.UniversityProfileCode = Trim(txtCode.Text)
        eobj.Name = Trim(txtName.Text)
        eobj.SName = Trim(txtSName.Text)
        eobj.Adress = Trim(txtAddress.Text)
        eobj.Adress1 = Trim(txtAddres1.Text)
        eobj.Adress2 = Trim(txtAddres2.Text)
        eobj.City = Trim(TxtCity.Text)
        eobj.Country = ddlCountry.SelectedIndex
        eobj.State = Trim(txtState.Text)
        eobj.PostCode = Trim(txtPostalCode.Text)
        eobj.Phone = Trim(txtPhone.Text)
        eobj.Fax = Trim(txtFax.Text)
        eobj.Email = Trim(txtEmail.Text)
        eobj.Website = Trim(txtWebSite.Text)
        eobj.UpdatedUser = Session("User")
        eobj.UpdatedDtTm = Date.Now.ToString()
        lblMsg.Visible = True
        If Session("PageMode") = "Add" Then
            Try
                RecAff = bsobj.Insert(eobj)
                ErrorDescription = "Record Saved Successfully "
                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                ErrorDescription = ex.Message.ToString
                lblMsg.Text = ErrorDescription
                lblMsg.Visible = True
                LogError.Log("UniversityProfile", "onSave", ex.Message)
            End Try
        ElseIf Session("PageMode") = "Edit" Then
            Try
                RecAff = bsobj.Update(eobj)
                ErrorDescription = "Record Updated Successfully "
                lblMsg.Text = ErrorDescription
            Catch ex As Exception
                ErrorDescription = ex.Message.ToString
                lblMsg.Text = ErrorDescription
                lblMsg.Visible = True
                LogError.Log("UniversityProfile", "onSave", ex.Message)
            End Try
        End If
    End Sub
    ''' <summary>
    ''' Method to Delete University Profile
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onDelete()
        lblMsg.Visible = True
        If txtCode.Text <> "" Then
            If lblCount.Text = "" Then lblCount.Text = 0
            If lblCount.Text > 0 Then
                Dim bsobj As New UniversityProfileBAL
                Dim eobj As New UniversityProfileEn
                Dim RecAff As Integer
                lblMsg.Visible = True
                eobj.UniversityProfileCode = Trim(txtCode.Text)
                Try
                    RecAff = bsobj.Delete(eobj)

                    ErrorDescription = "Record Deleted Successfully "
                    lblMsg.Text = ErrorDescription
                    lblMsg.Visible = True
                    DFlag = "Delete"

                Catch ex As Exception
                    lblMsg.Text = ex.Message.ToString()
                    LogError.Log("UniversityProfile", "onDelete", ex.Message)
                End Try
                txtCode.Text = ""
                txtName.Text = ""
                txtSName.Text = ""
                LoadListObjects()
            Else
                ErrorDescription = "Select a Record to Delete"
                lblMsg.Text = ErrorDescription
            End If
        Else
            ErrorDescription = "Select a Record to Delete"
            lblMsg.Text = ErrorDescription
        End If
    End Sub
    ''' <summary>
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub onClearData()
        txtCode.Enabled = True

        Session("ListObj") = Nothing
        txtCode.Text = ""
        txtName.Text = ""
        txtName.Text = ""
        txtSName.Text = ""
        txtAddress.Text = ""
        txtAddres1.Text = ""
        txtAddres2.Text = ""
        TxtCity.Text = ""
        ddlCountry.SelectedIndex = Nothing
        lblMsg.Text = ""
        txtState.Text = ""
        txtPostalCode.Text = ""
        txtPhone.Text = ""
        txtFax.Text = ""
        txtEmail.Text = ""
        txtWebSite.Text = ""
        'txtBCode.Text = ""
        DisableRecordNavigator()

    End Sub
    ''' <summary>
    '''  Method to Fill the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ViewData()
        Dim eobj As New UniversityProfileEn
        Dim bobj As New UniversityProfileBAL

        Try
            ListObjects = bobj.GetUniversityProfileList(eobj)
        Catch ex As Exception
            LogError.Log("UniversityProfile", "ViewData", ex.Message)
        End Try
        txtCode.Text = eobj.UniversityProfileCode
        txtName.Text = eobj.Name
        txtSName.Text = eobj.SName
        txtAddress.Text = eobj.Adress
        txtAddres1.Text = eobj.Adress1
        txtAddres2.Text = eobj.Adress2
        TxtCity.Text = eobj.City
        ddlCountry.SelectedIndex = eobj.Country
        txtState.Text = eobj.State
        txtPostalCode.Text = eobj.PostCode
        txtPhone.Text = eobj.Phone
        txtFax.Text = eobj.Fax
        txtEmail.Text = eobj.Email
        txtWebSite.Text = eobj.Website
        eobj = Nothing
    End Sub
    ''' <summary>
    ''' Method to Move to First Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveFirst()
        txtRecNo.Text = "1"
        FillData(0)
    End Sub
    ''' <summary>
    ''' Method to Move to Next Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveNext()
        txtRecNo.Text = CInt(txtRecNo.Text) + 1
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Previous Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMovePrevious()
        txtRecNo.Text = CInt(txtRecNo.Text) - 1
        FillData(CInt(txtRecNo.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Move to Last Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnMoveLast()
        txtRecNo.Text = lblCount.Text
        FillData(CInt(lblCount.Text) - 1)
    End Sub
    ''' <summary>
    ''' Method to Fill the Field Values
    ''' </summary>
    ''' <param name="RecNo"></param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal RecNo As Integer)
        lblMsg.Visible = False
        'Conditions for Button Enable & Disable
        If txtRecNo.Text = lblCount.Text Then
            ibtnNext.Enabled = False
            ibtnNext.ImageUrl = "images/gnew_next.png"
            ibtnLast.Enabled = False
            ibtnLast.ImageUrl = "images/gnew_last.png"
        Else
            ibtnNext.Enabled = True
            ibtnNext.ImageUrl = "images/new_next.png"
            ibtnLast.Enabled = True
            ibtnLast.ImageUrl = "images/new_last.png"
        End If
        If txtRecNo.Text = "1" Then
            ibtnPrevs.Enabled = False
            ibtnPrevs.ImageUrl = "images/gnew_Prev.png"
            ibtnFirst.Enabled = False
            ibtnFirst.ImageUrl = "images/gnew_first.png"
        Else
            ibtnPrevs.Enabled = True
            ibtnPrevs.ImageUrl = "images/new_prev.png"
            ibtnFirst.Enabled = True
            ibtnFirst.ImageUrl = "images/new_first.png"
        End If

        PnlAdd.Visible = True
        Dim obj As UniversityProfileEn
        ListObjects = Session("ListObj")
        obj = ListObjects(RecNo)
        txtCode.Text = obj.UniversityProfileCode
        txtName.Text = obj.Name
        txtSName.Text = obj.SName
        txtAddress.Text = obj.Adress
        txtAddres1.Text = obj.Adress1
        txtAddres2.Text = obj.Adress2
        TxtCity.Text = obj.City
        ddlCountry.SelectedIndex = obj.Country
        txtState.Text = obj.State
        txtPostalCode.Text = obj.PostCode
        txtPhone.Text = obj.Phone
        txtFax.Text = obj.Fax
        txtEmail.Text = obj.Email
        txtWebSite.Text = obj.Website

    End Sub
    ''' <summary>
    ''' Method to get the MenuName
    ''' </summary>
    ''' <param name="MenuId">Parameter is MenuId</param>
    ''' <remarks></remarks>
    Private Sub Menuname(ByVal MenuId As Integer)
        Dim eobj As New MenuEn
        Dim bobj As New MenuBAL
        eobj.MenuId = MenuId
        Try
            eobj = bobj.GetMenus(eobj)
        Catch ex As Exception
            LogError.Log("UniversityProfile", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
    ''' <summary>
    ''' Method to get the List Of University Profiles
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()
        Dim ds As New DataSet
        Dim bobj As New UniversityProfileBAL
        Dim eobj As New UniversityProfileEn

        eobj.UniversityProfileCode = txtCode.Text
        eobj.Name = txtName.Text
        eobj.SName = txtSName.Text

        Try
            ListObjects = bobj.GetUniversityProfileList(eobj)
        Catch ex As Exception
            LogError.Log("UniversityProfile", "LoadListObjects", ex.Message)
        End Try
        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()
        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()
            txtRecNo.Text = "1"
            PnlAdd.Visible = True
            OnMoveFirst()
            If Session("EditFlag") = True Then
                Session("PageMode") = "Edit"
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
                lblMsg.Visible = True
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
                lblMsg.Visible = True
            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            onClearData()
            If DFlag = "Delete" Then
                lblMsg.Text = "Record Deleted Successfully "
            Else
                lblMsg.Visible = True
                ErrorDescription = "Record did not Exist"
                lblMsg.Text = ErrorDescription
                DFlag = ""
            End If
        End If
    End Sub
#End Region

    Protected Sub txtName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.TextChanged

    End Sub
End Class
