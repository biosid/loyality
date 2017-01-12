<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RapidSoft.Etl.Monitor.Models.EtlPackageMonitorModel>" %>
<%@ Import Namespace="RapidSoft.Etl.Logging" %>
<%@ Import Namespace="RapidSoft.Etl.Monitor.Helpers" %>
<%@ Import Namespace="RapidSoft.Etl.Monitor.Models" %>
<%@ Import Namespace="RapidSoft.Etl.Monitor.Resources.Views.Shared" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server"><%: String.Concat("Мониторинг", Model.HasErrors ? " - есть ошибки" : String.Empty) %></asp:Content>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
<div class='menu'>
    <%: Html.ActionLink("Список сессий", "EtlSessionList", "EtlSessionList") %>
</div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
<section><h2 class="<%: Model.HasErrors ? "redtext" : "normal" %>"><%: String.Concat("Мониторинг", Model.HasErrors ? " - есть ошибки" : String.Empty)%></h2></section>

<%using (Html.BeginForm("EtlPackageMonitor", "EtlPackageMonitor", FormMethod.Get))
{%>
    <div class="etlsessions-div">
    <br />
    <table class="etlsessions-table" cellpadding="0" cellspacing="0">
        <tbody>
            <% for (var r = 0; r < Model.MaxItemCountInColumn; r++) { %>
            <tr>
                <td style="padding:0px;border:0px;"><div style="width:7px;height:1px"></div></td>
                <% for (var c = 0; c < Model.ColumnCount; c++) { %>
                    <% var item = r < Model.ItemsByColumns[c].Count ? Model.ItemsByColumns[c][r] : null; %>
                    <% if (item != null){ %>    
					<td style="border-right:0px;width:<%: Math.Round((100 / Model.ColumnCount) * 0.3) %>%" class="etlmonitor-etlpackagename <%: EtlPackageMonitorModel.GetEtlStatusClass(item.EtlPackageStatus) %>">
    					<strong><%= item.EtlPackageName %></strong><br /><br />
                        <% if (item.CanInvoke){ %>
						<%: Html.ActionLink("Выполнить", "InvokePackage", new {etlPackageId = item.EtlPackageId})%>
                        <% } else { %>&nbsp;<% } %>
					</td>
					<td style="border-left:0px;width:<%: Math.Round((100 / Model.ColumnCount) * 0.7) %>%" class="etlmonitor-status <%: EtlPackageMonitorModel.GetEtlStatusClass(item.EtlPackageStatus) %>">
                        <nobr><% if (item.EtlPackageStatus != EtlPackageMonitorItemStatus.Never){ %>
                        <%:Html.ActionLink(EtlPackageMonitorItemStatuses.ToString(item.EtlPackageStatus), "EtlSessionView", "EtlSessionView", new { packageId = item.EtlPackageId, sessionId = item.EtlSessionId, backUrl = Server.UrlEncode(Request.RawUrl) }, new object())%>
                        <% } else { %>
                        <%: EtlPackageMonitorItemStatuses.ToString(item.EtlPackageStatus)%>
                        <% } %></nobr><br />

                        <%: item.EtlSessionDateTime != null ? EtlPackageMonitorModel.ToIntelliDateTimeText(item.EtlSessionDateTime.Value, SharedStrings.DateTimeFormat) : ""%><br /><br />
                        <%: item.EtlPackageStatus != EtlPackageMonitorItemStatus.Succeeded ? item.StatusMessage : String.Empty%>

                        <% if (item.EtlPackageStatus != EtlPackageMonitorItemStatus.Failed && item.Counters != null && item.Counters.Length > 0) { %>
                        <table cellpadding="0" cellspacing="0" style="background-color:#ffffff;">
                            <tbody>
							<% foreach (var counterGroup in item.Counters.GroupBy(cnt => cnt.EntityName)) { %>
							    <tr>
								    <td style="font-weight:bold">
									    <%: counterGroup.Key %>
								    </td>
								    <td>&nbsp;</td>
							    </tr>                                  
                                <% foreach (var counter in counterGroup) { %>
							        <tr>
								        <td style="padding-left:20px;"><%:counter.CounterName %></td>
                                        <td><%:counter.CounterValue %></td>
							        </tr>                                            
                                <% } %>
                            <% } %>
                            </tbody>
                        </table>
                        <% } %>
					</td>                        
                    <% } else {%>    
					    <td class="etlmonitor-etlpackagename">&nbsp;</td>
					    <td class="etlmonitor-status">&nbsp;</td>
                    <% } %>    
                    <td style="padding:0px;border:0px;"><div style="width:7px;height:1px"></div></td>
                <% } %>    
            </tr>
            <tr>
                <td colspan="<%: Model.ColumnCount %>" style="padding:0px;border:0px;"><div style="width:1px;height:10px"></div></td>
            </tr>
            <% } %>
        </tbody>
    </table>

</div>
<%}%>
</asp:Content>