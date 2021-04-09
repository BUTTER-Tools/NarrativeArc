#  .o oOOOOOOOo                                            OOOo
#   Ob.OOOOOOOo  OOOo.      oOOo.                      .adOOOOOOO
#   OboO"""""""""""".OOo. .oOOOOOo.    OOOo.oOOOOOo.."""""""""'OO
#   OOP.oOOOOOOOOOOO "POOOOOOOOOOOo.   `"OOOOOOOOOP,OOOOOOOOOOOB'
#   `O'OOOO'     `OOOOo"OOOOOOOOOOO` .adOOOOOOOOO"oOOO'    `OOOOo
#   .OOOO'            `OOOOOOOOOOOOOOOOOOOOOOOOOO'            `OO
#   OOOOO                 '"OOOOOOOOOOOOOOOO"`                oOO
#  oOOOOOba.                .adOOOOOOOOOOba               .adOOOOo.
# oOOOOOOOOOOOOOba.    .adOOOOOOOOOO@^OOOOOOOba.     .adOOOOOOOOOOOO
#OOOOOOOOOOOOOOOOO.OOOOOOOOOOOOOO"`  '"OOOOOOOOOOOOO.OOOOOOOOOOOOOO
#OOOO"       "YOoOOOOMOIONODOO"`  .   '"OOROAOPOEOOOoOY"     "OOO"
#   Y           'OOOOOOOOOOOOOO: .oOOo. :OOOOOOOOOOO?'         :`
#   :            .oO%OOOOOOOOOOo.OOOOOO.oOOOOOOOOOOOO?         .
#   .            oOOP"%OOOOOOOOoOOOOOOO?oOOOOO?OOOO"OOo
#                '%o  OOOO"%OOOO%"%OOOOO"OOOOOO"OOO':
#                     `$"  `OOOO' `O"Y ' `OOOO'  o             .
#   .                  .     OP"          : o     .
#                            :
#-Correlation template
#-RYAN L. BOYD 
#----All input should be in .CSV format
#----All input MUST be in wide format for this script


setwd("C:/Users/Ryan/Desktop/Texts")
dir.create("Results", showWarnings = FALSE)


df.z.diff = read.csv('BUTTER-CSVOutput-z-diff.csv', fileEncoding = 'UTF-8-BOM')
df.z.frec = read.csv('BUTTER-CSVOutput-z-frech.csv', fileEncoding = 'UTF-8-BOM')
df.lf.diff = read.csv('BUTTER-CSVOutput-lf-diff.csv', fileEncoding = 'UTF-8-BOM')
df.lf.frec  = read.csv('BUTTER-CSVOutput-lf-frech.csv', fileEncoding = 'UTF-8-BOM')



prepDF = function(df, desc){
  df = subset(df, Tokens >= 250)
  df = df[ , c('TextID', 'Narrativity_Overall', 'Narrativity_Staging', 'Narrativity_PlotProg', 'Narrativity_CogTension')]
  colnames(df)[2:5] = paste(colnames(df)[2:5], desc, sep='.')
  return(df)
}

df.z.diff = prepDF(df.z.diff, 'z.diff')
df.z.frec = prepDF(df.z.frec, 'z.frec')
df.lf.diff = prepDF(df.lf.diff, 'lf.diff')
df.lf.frec = prepDF(df.lf.frec, 'lf.frech')

DF = merge(df.z.diff, df.z.frec, by='TextID', all=T)
DF = merge(DF, df.lf.diff, by='TextID', all=T)
DF = merge(DF, df.lf.frec, by='TextID', all=T)

BeginningColumn = 2




require(Hmisc)


RCorrFunction <- function (InputData) {
  model <- rcorr(as.matrix(InputData[BeginningColumn:length(InputData)]), type = "pearson")
  return(model)
}



Results = RCorrFunction(DF)

write.csv(Results$r, paste("Results/", Sys.Date(), "_-_Correlation_Coefficients.csv", sep=""), na='')
write.csv(Results$P, paste("Results/", Sys.Date(), "_-_Correlation_p-vals.csv", sep=""), na='')
write.csv(Results$n, paste("Results/", Sys.Date(), "_-_Correlation_NumObservations.csv", sep=""), na='')


