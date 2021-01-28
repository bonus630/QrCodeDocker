<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:frmwrk="Corel Framework Data">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <frmwrk:uiconfig>
   
    <frmwrk:applicationInfo userConfiguration="true" />
  </frmwrk:uiconfig>

  <!-- Copy everything -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="uiConfig/items">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
		<!-- Define the button will contains menu is same in all projects -->
		<itemData guid="f1d3d1d0-cc8d-4f04-91cb-7112255b8af1" noBmpOnMenu="true"
				  type="flyout"
				  dynamicCategory="2cc24a3e-fe24-4708-9a74-9c75406eebcd"
				  userCaption="Bonus630 Dockers"
				  enable="true"
				  flyoutBarRef="FB727225-CEA7-4D27-BB27-52C687B53029"
                />
      <!-- Define the button which shows the docker -->
      <itemData guid="DF67BEBE-6551-4F3B-BE5B-1BF46E16AB67" noBmpOnMenu="true"
                type="checkButton"
                check="*Docker('68622454-ABAA-4099-976F-E620DCF8C89B')"
                dynamicCategory="2cc24a3e-fe24-4708-9a74-9c75406eebcd"
                userCaption="QR Gerador de Código PT-BR"
                enable="true"/>

      <!-- Define the web control which will be placed on our docker -->
      <itemData guid="991EC505-20DD-48C5-8313-58C0E293C9C3"
                type="wpfhost"
                hostedType="Addons\QrCodeDocker\QrCodeDocker.dll,br.corp.bonus630.QrCodeDocker.Docker"
                enable="true"/>

    </xsl:copy>
  </xsl:template>
	<!-- Define the new menu is same in all others project-->
	<xsl:template match="uiConfig/commandBars">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>

			<commandBarData guid="FB727225-CEA7-4D27-BB27-52C687B53029"
							type="menu"
							nonLocalizableName="Bonus630 Dockers"
							flyout="true">
				<menu>

					<!--Here change to new item-->
					<!--<item guidRef="DF67BEBE-6551-4F3B-BE5B-1BF46E16AB67"/>-->

				</menu>
			</commandBarData>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="uiConfig/commandBars/commandBarData[guid='FB727225-CEA7-4D27-BB27-52C687B53029']/menu">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>

					<!--Here change to new item-->
					<item guidRef="DF67BEBE-6551-4F3B-BE5B-1BF46E16AB67"/>

		</xsl:copy>
	</xsl:template>
  <xsl:template match="uiConfig/dockers">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>

      <!-- Define the web docker -->
      <dockerData guid="68622454-ABAA-4099-976F-E620DCF8C89B"
                  userCaption="QR Gerador de Código PT-BR"
                  wantReturn="true"
                  focusStyle="noThrow">
        <container>
          <!-- add the webpage control to the docker -->
          <item dock="fill" margin="0,0,0,0" guidRef="991EC505-20DD-48C5-8313-58C0E293C9C3"/>
        </container>
      </dockerData>
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>
