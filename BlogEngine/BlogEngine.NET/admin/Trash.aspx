<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="Trash.aspx.cs" Inherits="Admin.Trash" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphAdmin" Runat="Server">
	<div class="content-box-outer">
		<div class="content-box-full">

            <h1>Trash</h1>
            <div class="tableToolBox">
                <div class="Pager"></div>
            </div>
            <div id="Container"></div>
            <div class="Pager"></div>

        </div>
    </div>

    <script type="text/javascript">
        LoadTrash(1);
    </script>

</asp:Content>
