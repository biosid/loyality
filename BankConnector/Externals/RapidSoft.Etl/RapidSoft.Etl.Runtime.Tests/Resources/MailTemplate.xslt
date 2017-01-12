<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
				exclude-result-prefixes="msxsl"
				xmlns:ext="http://rapidsoft.ru/ELA">

	<xsl:output
		media-type="text/html" method="html" encoding="utf-8"
		omit-xml-declaration="no"
		doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"
		doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"
	/>

	<xsl:param name="subject"/>

	<xsl:template match="/EtlMonitoringResult">
		<html xmlns="http://www.w3.org/1999/xhtml">
			<head>
				<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
				<title>
					<xsl:value-of select="$subject"/>
				</title>
				<style type="text/css">
					body, h1, h2, p, table, caption, tbody, tfoot, thead, tr, th, td { font-weight: inherit; font-style: inherit; font-size: 100%; vertical-align: baseline; margin: 0; padding: 0; border: 0; }
					body { padding: 5px; background: #FFF; color: #222; font-size: 75%; font-family: Tahoma, Arial, Helvetica, sans-serif; line-height: 1.5; }
					h1, h2 { font-weight: normal; }
					h1 { color: #F4A30E; font-size: 2em; line-height: 1.1; }
					h1 strong { color: #333; font-size: 0.7em; font-weight: normal; line-height: 1.7em; }
					h2 { color: #333; font-size: 1.5em; text-align: left; margin: 1.2em 0 0.3em 0; }
					p { margin: 0 0 0.8em; }
					table { border-collapse: collapse; margin: 0; }
					table table { width: 100%; }
					td { vertical-align: top; padding: 0 5px; }
					td td { background: #FFF; text-align: center; padding: 3px 10px; border: 1px solid #999; }
					.entity td, .entity th { border: 1px solid #999; padding: 3px 10px; }
					.entity tr:hover td { background-color: #FFFFCC; }
					table th { vertical-align: middle; }
					table td { text-align: left; font-weight: normal; font-family: Geneva, Arial, Helvetica, sans-serif; }
					.center { text-align: center; margin: 0 auto; }
					.left { text-align: left; }
					.today { text-align: right; margin: 5px 0 0; }
					.header td { text-align: left; border: 0; padding: 0; }
					.notvalid td, .notvalid th { background: #FF9999 !important; color: #000; }
					.package { }
					.session { border: none; }
					.session td { border: none; padding: 15px 0 15px; }
					.entity td { border: 1px solid #999; padding: 3px 5px; }
					.entity th { vertical-align: middle; background-color: #888888; color: #FFFFFF; }
					.entity td { text-align: left; font-weight: normal; font-family: Geneva, Arial, Helvetica, sans-serif; }
					.status { margin: 0 0 5px 8px; }
					.sessionTitle { margin: 0 0 0 1px; font-weight: bold; }
				</style>
			</head>
			<body>
				<h1>
					<xsl:value-of select="$subject"/>
					<br />
					<strong>
						<xsl:value-of select="CollectedDateTime"/>
					</strong>
				</h1>
				<table cellpadding="0" cellspacing="0">
					<xsl:apply-templates select="EtlPackageResults/EtlPackageResult" />
				</table>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="EtlPackageResults/EtlPackageResult">
		<tr>
			<td>
				<div class="package">
					<h2>
						<xsl:value-of select="EtlPackageHeader/DisplayName"/>
					</h2>
					<xsl:choose>
						<xsl:when test="EtlSessionResults/EtlSessionResult">
							<table cellpadding="0" cellspacing="0" class="entity" width="100%">
								<tr>
									<th>Сущность</th>
									<th>Статус</th>
									<th>Получено</th>
									<th>Отброшено</th>
									<th>Обработано</th>
									<th>Добавлено</th>
									<th>Обновлено</th>
									<th>Удалено</th>
									<th>Cообщение</th>
									<th>Сессия</th>
								</tr>
								<xsl:apply-templates select="EtlSessionResults/EtlSessionResult" />
							</table>
						</xsl:when>
						<xsl:otherwise>
							<div class="sessionTitle">
								Нет данных
							</div>
						</xsl:otherwise>
					</xsl:choose>
				</div>
				<br />
			</td>
		</tr>
	</xsl:template>

	<xsl:template match="EtlSessionResults/EtlSessionResult">
		<xsl:choose>
			<xsl:when test="EtlEntityLoadingResults/EtlEntityLoadingResult">
				<xsl:apply-templates select="EtlEntityLoadingResults/EtlEntityLoadingResult" />
			</xsl:when>
			<xsl:otherwise>
				<tr>
					<td colspan="9" align="center">Нет данных</td>
					<td>
						<xsl:attribute name="style">
							<xsl:text>font-weight:bold; background-color:</xsl:text>
							<xsl:choose>
								<xsl:when test="Status/text() = 'Succeeded'">
									<xsl:text>#CCFFCC;</xsl:text>
								</xsl:when>
								<xsl:when test="Status/text() = 'FinishedWithWarnings'">
									<xsl:text>#FFFFCC;</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>#FF9999;</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:choose>
							<xsl:when test="Status/text() = 'Started'">
								<xsl:text>Запущено</xsl:text>
							</xsl:when>
							<xsl:when test="Status/text() = 'Succeeded'">
								<xsl:text>Успешно завершено</xsl:text>
							</xsl:when>
							<xsl:when test="Status/text() = 'Failed'">
								<xsl:text>Завершено с ошибками</xsl:text>
							</xsl:when>
							<xsl:when test="Status/text() = 'FinishedWithLosses'">
								<xsl:text>Завершено с потерей данных</xsl:text>
							</xsl:when>
							<xsl:when test="Status/text() = 'FinishedWithWarnings'">
								<xsl:text>Завершено с предупреждениями</xsl:text>
							</xsl:when>
						</xsl:choose>
						<xsl:text> </xsl:text>
						<xsl:value-of select="StartDateTime"/>
						<xsl:if test="EndDateTime and string-length(EndDateTime) &gt; 0">
							<xsl:text> &#x2014; </xsl:text>
							<xsl:value-of select="EndDateTime"/>
						</xsl:if>
						<xsl:if test="StartMessage">
							<br />
							<xsl:value-of select="StartMessage"/>
						</xsl:if>
						<xsl:if test="EndMessage">
							<br />
							<xsl:value-of select="EndMessage"/>
						</xsl:if>
					</td>
				</tr>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="EtlEntityLoadingResults/EtlEntityLoadingResult">
		<tr>
			<xsl:choose>
				<xsl:when test="./../../preceding-sibling::EtlSessionResult[1]/EtlEntityLoadingResult/EtlEntityLoadingResults[position()]/EtlEntityName = current()/EtlEntityName">
					<td> </td>
				</xsl:when>
				<xsl:otherwise>
					<td>
						<xsl:value-of select="EtlEntityDisplayName"/>
					</td>
				</xsl:otherwise>
			</xsl:choose>
			<td>
				<xsl:attribute name="style">
					<xsl:text>background-color: </xsl:text>
					<xsl:call-template name="GetEntityColor">
						<xsl:with-param name="status" select="Status" />
					</xsl:call-template>
				</xsl:attribute>
				<xsl:choose>
					<xsl:when test="Status/text() = 'Absent'">
						<xsl:text>Не было обновлено</xsl:text>
					</xsl:when>
					<xsl:when test="Status/text() = 'Succeeded'">
						<xsl:text>Успешно завершено</xsl:text>
					</xsl:when>
					<xsl:when test="Status/text() = 'Failed'">
						<xsl:text>Завершено с ошибками</xsl:text>
					</xsl:when>
					<xsl:when test="Status/text() = 'FinishedWithLosses'">
						<xsl:text>Завершено с потерей данных</xsl:text>
					</xsl:when>
					<xsl:when test="Status/text() = 'FinishedWithWarnings'">
						<xsl:text>Завершено с предупреждениями</xsl:text>
					</xsl:when>
				</xsl:choose>
			</td>
			<td>
				<xsl:value-of select="SourceRowCount"/>
			</td>
			<td>
				<xsl:value-of select="ErrorSourceRowCount"/>
			</td>
			<td>
				<xsl:value-of select="AcceptedSourceRowCount"/>
			</td>
			<td>
				<xsl:value-of select="InsertedRowCount"/>
			</td>
			<td>
				<xsl:value-of select="UpdatedRowCount"/>
			</td>
			<td>
				<xsl:value-of select="DeletedRowCount"/>
			</td>
			<td>
				<xsl:value-of select="Message"/>
			</td>
			<td>
				<xsl:attribute name="style">
					<xsl:text>font-weight:bold; background-color:</xsl:text>
					<xsl:choose>
						<xsl:when test="./../../Status/text() = 'Succeeded'">
							<xsl:text>#CCFFCC;</xsl:text>
						</xsl:when>
						<xsl:when test="./../../Status/text() = 'FinishedWithWarnings'">
							<xsl:text>#FFFFCC;</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>#FF9999;</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:choose>
					<xsl:when test="./../../Status/text() = 'Started'">
						<xsl:text>Запущено</xsl:text>
					</xsl:when>
					<xsl:when test="./../../Status/text() = 'Succeeded'">
						<xsl:text>Успешно завершено</xsl:text>
					</xsl:when>
					<xsl:when test="./../../Status/text() = 'Failed'">
						<xsl:text>Завершено с ошибками</xsl:text>
					</xsl:when>
					<xsl:when test="./../../Status/text() = 'FinishedWithLosses'">
						<xsl:text>Завершено с потерей данных</xsl:text>
					</xsl:when>
					<xsl:when test="./../../Status/text() = 'FinishedWithWarnings'">
						<xsl:text>Завершено с предупреждениями</xsl:text>
					</xsl:when>
				</xsl:choose>
				<xsl:text> </xsl:text>
				<xsl:value-of select="./../../StartDateTime"/>
				<xsl:if test="./../../EndDateTime and string-length(./../../EndDateTime) &gt; 0">
					<xsl:text> &#x2014; </xsl:text>
					<xsl:value-of select="./../../EndDateTime"/>
				</xsl:if>
				<xsl:if test="./../../StartMessage">
					<br />
					<xsl:value-of select="./../../StartMessage"/>
				</xsl:if>
				<xsl:if test="./../../EndMessage">
					<br />
					<xsl:value-of select="./../../EndMessage"/>
				</xsl:if>
			</td>
		</tr>
	</xsl:template>

	<xsl:template name="GetEntityColor">
		<xsl:param name="status" />
		<xsl:choose>
			<xsl:when test="$status/text() = 'Absent' or $status/text() = 'Failed' or $status/text() = 'FinishedWithLosses'">
				<xsl:text>#FF9999;</xsl:text>
			</xsl:when>
			<xsl:when test="$status/text() = 'FinishedWithWarnings'">
				<xsl:text>#FFFFCC;</xsl:text>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>