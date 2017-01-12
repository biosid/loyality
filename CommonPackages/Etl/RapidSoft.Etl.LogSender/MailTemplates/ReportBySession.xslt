<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
				exclude-result-prefixes="msxsl"
>

  <xsl:output
		media-type="text/html" method="html" encoding="utf-8"
		omit-xml-declaration="no"
		doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"
		doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"
	/>

  <xsl:param name="subject"/>

  <xsl:key name="entityName" match="Counter/Counter" use="EntityName" />

  <xsl:template match="/EtlDump">
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
					h1 .EtlDumpDateTime { color: #333; font-size: 0.7em; font-weight: normal; line-height: 1.7em; }
					h2 { color: #333; font-size: 1.5em; text-align: left; margin: 1.2em 0 0.3em 0; }
					p { margin: 0 0 0.8em; }
					table { border-collapse: collapse; margin: 0; }
					table table { width: 100%; }
					td { vertical-align: top; padding: 0 5px; }
					td td { background: #FFF; text-align: center; padding: 3px 10px; border: 1px solid #999; }
					.entity td, .entity th { border: 1px solid #999; padding: 3px 10px; }
					.entity tr:hover td { background-color: #FFFFCC; }
					table td { text-align: left; font-weight: normal; font-family: Geneva, Arial, Helvetica, sans-serif; }
					table th { border: 1px solid #999; width: 10%; text-align: right; padding: 0 5px }
					.center { text-align: center; margin: 0 auto; }
					.left { text-align: left; }
					.today { text-align: right; margin: 5px 0 0; }
					.header td { text-align: left; border: 0; padding: 0; }
					.notvalid td, .notvalid th { background: #FF9999 !important; color: #000; }
					.package { }
					.EtlSessionsTable td { border: 1px solid #999; padding: 3px 5px; }
					.EtlSessionsTable th { vertical-align: middle; background-color: #888888; color: #FFFFFF; }
					.EtlSessionsTable td { text-align: left; font-weight: normal; font-family: Geneva, Arial, Helvetica, sans-serif; }
					.status { margin: 0 0 5px 8px; }
					.sessionTitle { margin: 0 0 0 1px; font-weight: bold; }
				</style>
      </head>
      <body>
        <h1>
          <xsl:value-of select="$subject"/>
          <br />
          <span class="EtlDumpDateTime">
            <xsl:call-template name="FormatDateTime">
              <xsl:with-param name="dt" select="DumpDateTime" />
            </xsl:call-template>
          </span>
        </h1>
        <br />
        <xsl:apply-templates select="Sessions/Session" />
      </body>
    </html>
  </xsl:template>

  <xsl:template match="Sessions/Session">
    <xsl:variable name="pid" select="EtlPackageId" />
    <h2>
      <xsl:choose>
        <xsl:when test="EtlPackageName != ''">
          <xsl:value-of select="EtlPackageName"/>
        </xsl:when>
        <xsl:otherwise>
          (неизвестно)
        </xsl:otherwise>
      </xsl:choose>
    </h2>
    <br />

		<table class="EtlSessionsTable" cellpadding="0" cellspacing="0" width="100%">
			<xsl:attribute name="style">
				<xsl:text>background-color: </xsl:text>
				<xsl:call-template name="GetSessionRowColor">
					<xsl:with-param name="status" select="Status" />
				</xsl:call-template>
			</xsl:attribute>
			<tr>
				<th>Id:</th>
				<td>
					<xsl:value-of select="EtlSessionId" />
				</td>
			</tr>
			<tr>
				<th>Начало:</th>
				<td>
					<xsl:call-template name="FormatDateTime">
						<xsl:with-param name="dt" select="StartDateTime" />
					</xsl:call-template>
				</td>
			</tr>
			<tr>
				<th>Окончание:</th>
				<td>
					<xsl:call-template name="FormatDateTime">
						<xsl:with-param name="dt" select="EndDateTime" />
					</xsl:call-template>
				</td>
			</tr>
			<tr>
				<th>Статус:</th>
				<td>
					<xsl:call-template name="GetSessionStatusDisplayName">
						<xsl:with-param name="status" select="Status" />
					</xsl:call-template>
				</td>
			</tr>
			<tr>
				<th>Результат:</th>
				<td>
					<xsl:choose>
						<xsl:when test="Status='Failed'">
							<xsl:value-of select="LastErrorMessage/Text" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:for-each select="Counters/Counter">
								<xsl:if test="not(EntityName = preceding-sibling::EntityName)">
									<xsl:if test="position() > 1">
										<br />
									</xsl:if>
									<b>
										<xsl:value-of select="EntityName"/> - 
									</b>
								</xsl:if>
								<xsl:value-of select="CounterName"/>: <xsl:value-of select="CounterValue"/>
							</xsl:for-each>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="Messages/Message/MessageType='Error'">
						<div style="color:#991E4B">
							<xsl:for-each select="Messages/Message[contains(MessageType, 'Error')]">
								<xsl:value-of select="Text"/>
								<br/>
							</xsl:for-each>
						</div>
					</xsl:if>
				</td>
			</tr>
		</table>
		<br />
  </xsl:template>

  <xsl:template name="FormatDateTime">
    <xsl:param name="dt" />
    <xsl:choose>
      <xsl:when test="$dt != ''">
        <xsl:value-of select="substring($dt, 9, 2)"/>.<xsl:value-of select="substring($dt, 6, 2)"/>.<xsl:value-of select="substring($dt, 1, 4)"/>&#160;<xsl:value-of select="substring($dt, 12, 5)"/>
      </xsl:when>
      <xsl:otherwise></xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="GetSessionStatusDisplayName">
    <xsl:param name="status" />
    <xsl:choose>
      <xsl:when test="$status/text() = 'Started'">
        <xsl:text>Запущено</xsl:text>
      </xsl:when>
      <xsl:when test="$status/text() = 'Succeeded'">
        <xsl:text>Успешно завершено</xsl:text>
      </xsl:when>
      <xsl:when test="$status/text() = 'Failed'">
        <xsl:text>Завершено с ошибками</xsl:text>
      </xsl:when>
      <xsl:when test="$status/text() = 'FinishedWithLosses'">
        <xsl:text>Завершено с потерей данных</xsl:text>
      </xsl:when>
      <xsl:when test="$status/text() = 'FinishedWithWarnings'">
        <xsl:text>Завершено с проблемами</xsl:text>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="GetSessionRowColor">
    <xsl:param name="status" />
    <xsl:choose>
      <xsl:when test="$status/text() = 'Failed' or $status/text() = 'FinishedWithWarnings' or $status/text() = 'FinishedWithLosses'">
        <xsl:text>#FF9999;</xsl:text>
      </xsl:when>
      <xsl:when test="$status/text() = 'Started'">
        <xsl:text>#FFFFCC;</xsl:text>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>