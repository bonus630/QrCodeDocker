<?xml version="1.0"?>

<xsl:stylesheet version="1.0"
								xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
								xmlns:frmwrk="Corel Framework Data"
								exclude-result-prefixes="frmwrk">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <!-- Use these elements for the framework to move the container from the app config file to the user config file -->
  <!-- Since these elements use the frmwrk name space, they will not be executed when the XSLT is applied to the user config file -->
  <frmwrk:uiconfig>
    <!-- The Application Info should always be the topmost frmwrk element -->
    <frmwrk:compositeNode xPath="/uiConfig/commandBars/commandBarData[@guid='cdb9ea9c-223e-4865-97a6-31c007c69674']"/>
	  <!-- Ours command bar frmwrk element-->
	  <frmwrk:compositeNode xPath="/uiConfig/commandBars/commandBarData[@guid='8baeb154-f910-46cd-b05c-3cf2808d1391']"/>
    <!--<frmwrk:compositeNode xPath="/uiConfig/commandBars/commandBarData[@guid='f455d4da-90b9-45c9-aaf9-77aa5f69384d']"/>-->
    <frmwrk:compositeNode xPath="/uiConfig/frame"/>
  </frmwrk:uiconfig>
	<!-- Copy everything -->
	<xsl:template match="node()|@*">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>
		</xsl:copy>
	</xsl:template>
	<!-- Helper to insert a new item into a menu/toolbar -->
	<xsl:template match="node()|@*" mode="insert-item">
		<xsl:param name="after"></xsl:param>
		<xsl:param name="before"></xsl:param>
		<xsl:param name="content"></xsl:param>
		<xsl:copy>
			<xsl:apply-templates select="@*"/>
			<xsl:for-each select="node()">
				<xsl:if test="name()='item' and @guidRef=$before">
					<xsl:copy-of select="$content"/>
				</xsl:if>
				<xsl:copy>
					<xsl:apply-templates select="node()|@*"/>
				</xsl:copy>
				<xsl:if test="name()='item' and @guidRef=$after">
					<xsl:copy-of select="$content"/>
				</xsl:if>
			</xsl:for-each>
			<xsl:if test="not(./item[@guidRef=$after]) and not(./item[@guidRef=$before])">
				<xsl:copy-of select="$content"/>
			</xsl:if>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="commandBarData[@guid='cdb9ea9c-223e-4865-97a6-31c007c69674']/menu">
		<xsl:apply-templates  mode="insert-item" select=".">
			
			<xsl:with-param name="content">
				
		<!-- Make sure we don't read the menu item it is already exists -->
		<xsl:if test="not(./item[@guidRef='61fea129-6e7a-46be-bfac-49075cfb97d8'])">
			<item guidRef="61fea129-6e7a-46be-bfac-49075cfb97d8"/>
		</xsl:if>
		</xsl:with-param>
		<xsl:with-param name="after" select = "'47cc9a2d-0b7a-4df1-a686-ea3aa21b4631'"/>
			
		</xsl:apply-templates>
	</xsl:template>
	
	<!-- Put the new command at the end of the 'CorelNaVeia dockers' menu -->
	<xsl:template match="commandBarData[@guid='8baeb154-f910-46cd-b05c-3cf2808d1391']/menu">
		<xsl:apply-templates  mode="insert-item" select=".">
			<xsl:with-param name="content">
			<!-- Make sure we don't read the menu item it is already exists -->
			<xsl:if test="not(./item[@guidRef='7836b4c1-0d32-4da0-a15f-68060f29648a'])">
				<item guidRef="7836b4c1-0d32-4da0-a15f-68060f29648a"/>
			</xsl:if>
			</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
  
</xsl:stylesheet>