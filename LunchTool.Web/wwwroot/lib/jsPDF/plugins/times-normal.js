﻿(function (jsPDFAPI) {
var callAddFont = function () {
this.addFileToVFS('times-normal.ttf', font);
this.addFont('times-normal.ttf', 'times', 'normal');
};
jsPDFAPI.events.push(['addFonts', callAddFont])
 })(jsPDF.API);