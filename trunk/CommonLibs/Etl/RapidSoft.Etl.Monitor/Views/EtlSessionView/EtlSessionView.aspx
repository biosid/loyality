<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RapidSoft.Etl.Monitor.Models.EtlSessionViewModel>" %>
<%@ Import Namespace="RapidSoft.Etl.Logging" %>
<%@ Import Namespace="RapidSoft.Etl.Logging.Dumps" %>
<%@ Import Namespace="RapidSoft.Etl.Monitor.Helpers" %>
<%@ Import Namespace="RapidSoft.Etl.Monitor.Models" %>
<%@ Import Namespace="RapidSoft.Etl.Monitor.Resources.Views.Shared" %>
<%@ Import Namespace="PagedList" %>
<%@ Import Namespace="PagedList.Mvc" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">Информация о загрузке</asp:Content>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeaderContent" runat="server">
<div class='menu'>
    <%: Html.ActionLink("Мониторинг", "EtlPackageMonitor", "EtlPackageMonitor")%>
</div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

	<section class="edit">
		<% using (Html.BeginForm("Back", "EtlSessionView", new{backUrl=Model.BackUrl})) { %>
			<div class="box">
				<div class="x1">
					<div class="x2">
						<div class="x3 <%: Model.Session != null ? EtlSessionViewModel.GetEtlStatusClass(Model.Session.Status) : "" %>">
        					<span class="right">
								<input type="submit" value="Назад" name="backButton" class="button5"/>
							</span>
                            <% if (Model.NotFound) { %>
                                Информация отсутствует
                            <% } else { %>
                                <h2>
                                <%: string.IsNullOrEmpty(Model.Session.EtlPackageName) ? "(неизвестно)" : Model.Session.EtlPackageName %>
                                </h2>
                                <span><%:EtlStatuses.ToString(Model.Session.Status)%></span>. 
                                Начало:&nbsp;<nobr><%= Model.Session.StartDateTime != null ?  Model.Session.StartDateTime.Value.ToString(SharedStrings.DateTimeFormat) : "(неизвестно)"%></nobr>, 
                                окончание:&nbsp;<nobr><%= Model.Session.EndDateTime != null ? Model.Session.EndDateTime.Value.ToString(SharedStrings.DateTimeFormat) : "(неизвестно)"%>.</nobr>
                            <% }%>
						 </div>
					</div>
				</div>
			</div>
		<% } %>
	</section>

	<% if (!Model.NotFound) { %>
		<div id="tabs">
			<ul>
				<li><a href="#tabs-1">Результаты</a></li>
				<li><a href="#tabs-2">Лог</a></li>
				<li><a href="#tabs-3">Переменные</a></li>
				<li><a href="#tabs-4">Дампы</a></li>
			</ul>
			<div id="tabs-1">
				<section class="edit">
					<table class="table2">
						<tbody>
							<% foreach (var counterGroup in Model.Session.Counters.GroupBy(cnt => cnt.EntityName)) { %>
							    <tr>
								    <td style="font-weight:bold"><%: counterGroup.Key %></td>
                                    <td>&nbsp;</td>
							    </tr>                                  
                                <% foreach (var counter in counterGroup) { %>
							        <tr>
								        <td style="padding-left:20px;"><%: counter.CounterName %>: <%: counter.CounterValue %></td>
								        <td><nobr><%: counter.DateTime.ToString(SharedStrings.DateTimeFormat) %></nobr></td>
							        </tr>                                            
                                <% } %>
                            <% } %>
                            <% if (Model.Session.LastErrorMessage != null) { %>
							<tr class="<%: EtlSessionViewModel.GetEtlStatusClass(Model.Session.Status) %>">
								<td>
									Произошла ошибка.<br /><%: Model.Session.LastErrorMessage.Text %>
								</td>
								<td>
									<nobr><%: Model.Session.LastErrorMessage.LogDateTime.ToString(SharedStrings.DateTimeFormat) %></nobr>
								</td>
							</tr> 
                            <% } %>
						</tbody>
					</table>
				</section>    
			</div>
			<div id="tabs-2">
				<section class="edit">
				    <%
                        var list = new StaticPagedList<EtlMessage>(this.Model.Session.Messages, this.Model.Page, this.Model.PageSize, this.Model.Session.MessageTotalCount);
				        Func<int, string> generatePageUrl = 
                            p => Url.Action("EtlSessionView", new   {
				                                                        packageId = this.Model.Session.EtlPackageId, 
                                                                        sessionId = this.Model.Session.EtlSessionId, 
                                                                        backUrl = this.Model.BackUrl, 
                                                                        page = p
				                                                    });
                     %>
                    <%= Html.PagedListPager(list, generatePageUrl) %>
					<table class="table2">
						<thead>
							<tr>
								<th>
									<div><span>N</span></div>
								</th>
								<th>
									<div><span>Время</span></div>
								</th>
								<th>
									<div><span>Шаг</span></div>
								</th>
								<th>
									<div><span>Тип</span></div>
								</th>
								<th>
									<div><span>Сообщение</span></div>
								</th>
								<th>
									<div><span>Отладочная информация</span></div>
								</th>
							</tr>
						</thead>
						<tbody>
							<% foreach (var msg in list) { %>
							    <tr class="<%: EtlSessionViewModel.GetEtlMessageClass(msg.MessageType) %>">
								    <td>
									    <%:msg.SequentialId %>
								    </td>
								    <td>
									    <nobr><%:msg.LogDateTime.ToString(SharedStrings.DateTimeFormat)%></nobr>
								    </td>
								    <td>
									    <%:msg.EtlStepName %>
								    </td>
								    <td>
									    <%: EtlMessageTypes.ToString(msg.MessageType) %>
								    </td>
								    <td>
									    <%:msg.Text %>
								    </td>
								    <td>
									    <%: msg.StackTrace %>
								    </td>
							    </tr>
                            <% } %>
						</tbody>
					</table>
                    <%= Html.PagedListPager(list, generatePageUrl) %>
				</section>
			</div>
			<div id="tabs-3">
				<section class="edit">
					<table class="table2">
						<thead>
							<tr>
								<th>
									<div><span>Время</span></div>
								</th>
								<th>
									<div><span>Имя</span></div>
								</th>
								<th>
									<div><span>Тип</span></div>
								</th>
								<th>
									<div><span>Значение</span></div>
								</th>
							</tr>
						</thead>
						<tbody>
							<% foreach (var variable in Model.Session.Variables) { %>
								<tr>
									<td>
										<nobr><%:variable.DateTime.ToString(SharedStrings.DateTimeFormat)%></nobr>
									</td>
									<td>
										<%:variable.Name %>
									</td>
									<td>
										<%:variable.Modifier %>
									</td>
									<td>
										<%:variable.IsSecure ? "(скрыто)" : variable.Value %>
									</td>
								</tr>
							<% } %>
						</tbody>
					</table>
				</section>
			</div>
			<div id="tabs-4">
                <section class="edit">
                    <%: Html.ActionLink("Скачать дамп (без данных)", "DownloadSessionDump", new {packageId = Model.Session.EtlPackageId, sessionId = Model.Session.EtlSessionId}) %>
                </section>
			</div>
		</div>
	<% } %>

</asp:Content>
<asp:Content ID="BottomContent" runat="server" ContentPlaceHolderID="BodyBottomContent">

	<script language="javascript" type="text/javascript">
		(function ($) {

		    $("#tabs").tabs({ selected: <%= this.Model.PageFromRequest ? 1 : 0 %> });
		})(jQuery);
	</script>

</asp:Content>
