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
    <!--<frmwrk:compositeNode xPath="/uiConfig/commandBars/commandBarData[@guid='3eaa9bbe-28fd-4672-9128-02974ee96332']"/>-->
    <frmwrk:compositeNode xPath="/uiConfig/commandBars/commandBarData[@guid='ce4f5308-94b6-465b-b3c5-5650c7038025']"/>
    <frmwrk:compositeNode xPath="/uiConfig/frame"/>
  </frmwrk:uiconfig>

  <!-- Copy everything -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>
<!--/uiConfig/commandBars/commandBarData[@guid='ce4f5308-94b6-465b-b3c5-5650c7038025']/menu/item[@guidRef='8a371e4d-cb69-4f90-47cb-dbc105f259cf']-->
  <!-- Put the new command at the end of the 'dockers' menu -->
  <xsl:template match="commandBarData[@guid='ce4f5308-94b6-465b-b3c5-5650c7038025']/menu/item[@guidRef='8a371e4d-cb69-4f90-47cb-dbc105f259cf']">
    <xsl:copy-of select="."/>
    <xsl:if test="not(./item[@guidRef='DF67BEBE-6551-4F3B-BE5B-1BF46E16AB67'])">
      <item guidRef="DF67BEBE-6551-4F3B-BE5B-1BF46E16AB67"/>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>


