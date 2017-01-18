Imports HTS.SAS.BusinessObjects
Imports HTS.SAS.DataAccessObjects
Imports HTS.SAS.Entities
Imports System.Data
Imports System.Collections.Generic

Partial Class CFIntegrationPrefixSetup
    Inherits System.Web.UI.Page

    Dim DFlag As String
    Dim ListObjects As List(Of SagaEn)
    Private ErrorDescription As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack() Then

            ibtnSave.Attributes.Add("onclick", "return validate()")
            ibtnDelete.Attributes.Add("onclick", "return getconfirm()")
            txtRecNo.Attributes.Add("OnKeyUp", "return geterr()")
            Session("PageMode") = ""
            LoadUserRights()

            Session("ListObj") = Nothing
            DisableRecordNavigator()
            Menuname(CInt(Request.QueryString("Menuid")))

        End If

        lblMsg.Text = ""
        lblMsg.Visible = False

    End Sub

    Protected Sub IbtnView_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnView.Click

        If lblCount.Text <> "" Then
            If CInt(lblCount.Text) > 0 Then
                OnClearData()
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

    Protected Sub ibtnSave_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSave.Click
        SpaceValidation()
    End Sub

    Protected Sub ibtnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnNew.Click
        OnAdd()
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

    Protected Sub txtRecNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtRecNo.TextChanged
        If Trim(txtRecNo.Text).Length = 0 Then
            txtRecNo.Text = 0
            If lblCount.Text <> Nothing Then
                If CInt(txtRecNo.Text) > CInt(lblCount.Text) Then
                    txtRecNo.Text = lblCount.Text
                End If
                FillData(CInt(txtRecNo.Text) - 1)
            Else
                txtRecNo.Text = ""
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

#Region "Methods"

    ''' <summary>
    ''' Method to Validate
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SpaceValidation()
        If Trim(txtPrefix.Text).Length >= 10 Then
            lblMsg.Text = "Prefix Should Not Be More Than 10 Characters"
            lblMsg.Visible = True
            txtPrefix.Focus()
            Exit Sub
        End If

        If ddlPostingType.SelectedValue = "-1" Then
            lblMsg.Text = "Posting Type Must Be Selected To Proceed"
            lblMsg.Visible = True
            Exit Sub
        End If

        OnSave()
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
            LogError.Log("CFIntegrationPrefix", "LoadUserRights", ex.Message)
        End Try

        'Rights for Add
        If eobj.IsAdd = True Then
            OnAdd()
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
    ''' Method to Clear the Field Values
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnClearData()
        txtPrefix.Enabled = True
        ddlPostingType.Enabled = True
        Session("ListObj") = Nothing
        DisableRecordNavigator()
        txtPrefix.Text = ""
        ddlPostingType.SelectedValue = "-1"
        lblMsg.Text = ""
        Session("PageMode") = "Add"
    End Sub

    ''' <summary>
    ''' Method to Load Fields in New Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnAdd()
        Session("PageMode") = "Add"
        PnlAdd.Visible = True
        OnClearData()
    End Sub

    ''' <summary>
    ''' Method to Change to Edit Mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnEdit()
        Session("PageMode") = "Edit"
        ddlPostingType.Enabled = False
    End Sub

    ''' <summary>
    ''' Method to Save and Update Student Category
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OnSave()
        Dim eobj As New SagaEn
        Dim RecAff As Integer

        eobj.Auto_Prefix = Trim(txtPrefix.Text)
        eobj.Posting_Type = Trim(ddlPostingType.SelectedValue)

        lblMsg.Visible = True

        If Session("PageMode") = "Add" Or Session("PageMode") = "Edit" Then
            Try
                RecAff = New SagaDAL().Insert(eobj)
                ErrorDescription = "Record Saved Successfully "
                lblMsg.Text = ErrorDescription

            Catch ex As Exception
                lblMsg.Text = ex.Message.ToString()
                LogError.Log("CFIntegrationPrefix", "OnSave", ex.Message)
            End Try

        End If

    End Sub

    ''' <summary>
    ''' Method to get the List Of Student Category
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadListObjects()

        Dim eobj As New SagaEn

        eobj.Posting_Type = ddlPostingType.SelectedValue
        Try
            ListObjects = New SagaDAL().GetList(eobj)
        Catch ex As Exception
            LogError.Log("CFIntegrationPrefix", "LoadListObjects", ex.Message)
        End Try

        Session("ListObj") = ListObjects
        lblCount.Text = ListObjects.Count.ToString()

        If ListObjects.Count <> 0 Then
            DisableRecordNavigator()

            txtRecNo.Text = 1

            OnMoveFirst()
            If Session("EditFlag") = True Then

                Session("PageMode") = "Edit"
                ddlPostingType.Enabled = False
                lblMsg.Visible = True
                ibtnSave.Enabled = True
                ibtnSave.ImageUrl = "images/save.png"
            Else
                Session("PageMode") = ""
                ibtnSave.Enabled = False
                ibtnSave.ImageUrl = "images/gsave.png"
            End If
        Else
            txtRecNo.Text = ""
            lblCount.Text = ""
            ddlPostingType.Enabled = True
            Session("ListObj") = Nothing
            DisableRecordNavigator()
            txtPrefix.Text = ""
            ddlPostingType.SelectedValue = "-1"
            Session("PageMode") = "Add"

            If DFlag = "Delete" Then
            Else
                lblMsg.Visible = True
                ErrorDescription = "Record did not Exist"
                lblMsg.Text = ErrorDescription
                DFlag = ""
            End If
        End If

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
    ''' Method to Fill the Field Values
    ''' </summary>
    ''' <param name="RecNo"></param>
    ''' <remarks></remarks>
    Private Sub FillData(ByVal RecNo As Integer)

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

        If txtRecNo.Text = 0 Then
            txtRecNo.Text = 1
        Else
            If lblCount.Text = 0 Then
                txtRecNo.Text = 0
            Else
                PnlAdd.Visible = True

                Dim obj As SagaEn
                ListObjects = Session("ListObj")
                obj = ListObjects(RecNo)

                txtPrefix.Text = obj.Auto_Prefix
                ddlPostingType.SelectedValue = obj.Posting_Type
            End If
        End If

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
            LogError.Log("CFIntegrationPrefix", "Menuname", ex.Message)
        End Try
        lblMenuName.Text = eobj.MenuName
    End Sub
#End Region

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        LoadUserRights()
        OnClearData()
    End Sub

End Class