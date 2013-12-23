<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:wix="http://schemas.microsoft.com/wix/2006/wi"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/wix:Wix">
      <xsl:processing-instruction name="include">Variables.wxi</xsl:processing-instruction>
      <xsl:copy>
        <xsl:apply-templates />
      </xsl:copy>
    </xsl:template>

  <xsl:template match="//wix:DirectoryRef[@Id='TARGETDIR']">
      <wix:DirectoryRef Id="StudioShellApplicationFolder">
        
          <xsl:apply-templates select="./*" />
        
      </wix:DirectoryRef>
    
  </xsl:template>
  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>
