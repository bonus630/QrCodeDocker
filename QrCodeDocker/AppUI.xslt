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
			<itemData guid="61fea129-6e7a-46be-bfac-49075cfb97d8" noBmpOnMenu="true"
					  type="flyout"
					  dynamicCategory="2cc24a3e-fe24-4708-9a74-9c75406eebcd"
					  userCaption="CorelNaVeia Dockers"
					  enable="true"
					  flyoutBarRef="8baeb154-f910-46cd-b05c-3cf2808d1391"
					  icon="guid://bffeb385-4c48-4fa9-b285-3d32e81030a9"
			/>

			<!-- Define the button which shows the docker -->
			<itemData guid="7836b4c1-0d32-4da0-a15f-68060f29648a" noBmpOnMenu="true"
					  type="checkButton"
					  check="*Docker('68622454-ABAA-4099-976F-E620DCF8C89B')"
					  dynamicCategory="2cc24a3e-fe24-4708-9a74-9c75406eebcd"
					  userCaption="QR Code Master Generator"
					  iconRef="efc02df4-8eb5-44b5-8016-1c495af1504e"
					  enable="true">

			</itemData>


			<!-- Define the web control which will be placed on our docker -->
			<itemData guid="efc02df4-8eb5-44b5-8016-1c495af1504e"
					  type="wpfhost"
					  hostedType="Addons\QrCodeDocker\QrCodeDocker.dll,br.corp.bonus630.QrCodeDocker.Docker"
					  enable="true"
					  icon="guid://bffeb385-4c48-4fa9-b285-3d32e81030a9"
					  width="326"
			  DocumentActive="Teste"
					  />




		</xsl:copy>
	</xsl:template>
	<!-- Define the new menu is same in all others project-->
	<xsl:template match="uiConfig/commandBars">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>

			<commandBarData guid="8baeb154-f910-46cd-b05c-3cf2808d1391"
							type="menu"
							nonLocalizableName="CorelNaVeia Dockers"
							icon="guid://bffeb385-4c48-4fa9-b285-3d32e81030a9"
							flyout="true">
				<menu>

					<!--Here change to new item-->
					<!--<item guidRef="7836b4c1-0d32-4da0-a15f-68060f29648a"/>-->

				</menu>
			</commandBarData>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="uiConfig/commandBars/commandBarData[guid='8baeb154-f910-46cd-b05c-3cf2808d1391']/menu">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>

			<!--Here change to new item-->
			<item guidRef="7836b4c1-0d32-4da0-a15f-68060f29648a"/>

		</xsl:copy>
	</xsl:template>
	<xsl:template match="uiConfig/dockers">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>

			<!-- Define the web docker -->
			<dockerData guid="68622454-ABAA-4099-976F-E620DCF8C89B"
						userCaption="QR Code Master Generator "
						noPadding='true'
						wantReturn="true"
						focusStyle="noThrow"
					icon="guid://bffeb385-4c48-4fa9-b285-3d32e81030a9">
				<container>
					<!-- add the webpage control to the docker -->
					<item dock="fill" margin="0,0,0,0" guidRef="efc02df4-8eb5-44b5-8016-1c495af1504e" />
				</container>
			</dockerData>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>
