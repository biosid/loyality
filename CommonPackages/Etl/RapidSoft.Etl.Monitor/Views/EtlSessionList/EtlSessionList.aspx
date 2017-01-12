<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RapidSoft.Etl.Monitor.Models.EtlSessionListModel>" %>
<%@ Import Namespace="RapidSoft.Etl.Logging" %>
<%@ Import Namespace="RapidSoft.Etl.Monitor.Helpers" %>
<%@ Import Namespace="RapidSoft.Etl.Monitor.Models" %>
<%@ Import Namespace="RapidSoft.Etl.Monitor.Resources.Views.Shared" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Загрузки</asp:Content>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
<div class='menu'>
    <%: Html.ActionLink("Мониторинг", "EtlPackageMonitor", "EtlPackageMonitor")%>
</div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
<section>
<h2>Загрузки</h2>
</section>

<%using (Html.BeginForm("EtlSessionList", "EtlSessionList", FormMethod.Get))
{%>
	<p class="filter">
		<label>&nbsp;Пакет:&nbsp;
			<%: Html.DropDownListFor(m => m.EtlPackagesId, this.Model.EtlPackages, p => new SelectListItem {Value = p.Id, Text = p.Name}, "Любой", "0") %>
		</label>
        <label>&nbsp;С даты:&nbsp;
			<%: Html.DateBoxFor(m => m.DateFrom, SharedStrings.DateFormat)%>
		</label>
		<label>&nbsp;по дату:&nbsp;
			<%: Html.DateBoxFor(m => m.DateTo, SharedStrings.DateFormat)%>
		</label>
        <label>&nbsp;Статус:&nbsp;
			<%: Html.DropDownForEnum<EtlSessionListModel, EtlStatus>(m => m.Status, EtlStatuses.ToString, "Любой", 0)%>
		</label>
		&nbsp;<input type="submit" class="button3" name="refresh" value="Обновить" />   
		<%: Html.ValidationMessageFor(m => m.DateFrom, "<br />", "")%>
		<%: Html.ValidationMessageFor(m => m.DateTo, "<br />", "")%>
	</p>
    <div class="etlsessions-div">
    <table class="etlsessions-table" cellpadding="0" cellspacing="0">
        <thead>
            <tr>
                <th class="etlsessions-startdate">
                    <div><span>Дата начала</span></div>
                </th>
                <th class="etlsessions-enddate">
                    <div><span>Дата окончания</span></div>
                </th>
                <th class="etlsessions-etlpackagename">
                    <div><span>Описание</span></div>
                </th>
                <th class="etlsessions-status">
                    <div><span>Статус</span></div>
                </th>					
            </tr>
        </thead>
        <tbody>
            <% foreach (var item in Model.Sessions) { %>
				<tr class="<%: EtlSessionListModel.GetEtlStatusClass(item.Status) %>">
					<td class="etlsessions-startdate">
					    <nobr><%= item.StartDateTime != null ? item.StartDateTime.Value.ToString(SharedStrings.DateTimeFormat) : "&mdash;"%></nobr>
					</td>				
					<td class="etlsessions-enddate">
						<nobr><%= item.EndDateTime != null ? item.EndDateTime.Value.ToString(SharedStrings.DateTimeFormat) : "&mdash;"%></nobr>
					</td>                
					<td class="etlsessions-etlpackagename">
						<%: item.EtlPackageName %>
					</td>
					<td class="etlsessions-status">
						<%:Html.ActionLink(EtlStatuses.ToString(item.Status), "EtlSessionView", "EtlSessionView", new {packageId = item.EtlPackageId, sessionId = item.EtlSessionId, backUrl = Server.UrlEncode(Request.RawUrl)}, new object()) %>
					</td>				
				</tr>
            <% } %>
        </tbody>
    </table>
	<% if(Model.Sessions.Count == Model.DefaultMaxSessionCount && Model.DateFrom != Model.DateTo) { %>
		<p class="maxrows">
			Показаны не все записи, так количество отображаемых записей ограничено и равно <%: Model.DefaultMaxSessionCount%>. Попробуйте задать другой временной интервал, чтобы увидеть остальные записи.
		</p>
	<% } %>
</div>
<%}%>
</asp:Content>
<asp:Content ID="BottomContent" runat="server" ContentPlaceHolderID="BodyBottomContent">
<script language="javascript" type="text/javascript">
(function ($) {

    $.datepicker.setDefaults($.datepicker.regional['ru']);
    var datepickerDefaults = {
        buttonImageOnly: true,
        buttonImage: '/Content/images/calendar.png',
        buttonText: 'Calendar',
        showOn: 'button',
        minDate: new Date(2010, 0, 1),
        maxDate: new Date(2900, 0, 1)
    }
    $.datepicker.setDefaults(datepickerDefaults);

    $('#DateFrom, #DateTo').datepicker
    (
        {
            onClose: function (dateText, inst) { this.form.submit(); }
        }
    );

})(jQuery);
</script>
</asp:Content>
