var PROXCardPrintAcknowledgementSDK = window.PROXCardPrintAcknowledgementSDK || {};


PROXCardPrintAcknowledgementSDK.onAction = function (formContext) {
    console.log(formContext);
    //var formContext = executionContext.getFormContext();
    const No = formContext.getAttribute("clp_name").getValue();
    const ReturnDate = PROXCardPrintAcknowledgementSDK.dateFormat("dd.mm.YYYY",formContext.getAttribute("clp_returndate").getValue());
    //const UserId = Xrm.Utility.getGlobalContext().userSettings.userId.replace(/\{|}/g, '').toLowerCase();
    const doc = new window.parent.jspdf.jsPDF();
    const ToName = formContext.getAttribute("clp_holder").getValue()?.[0].name;
    //var ToName = formContext.getAttribute("clp_holder").getValue() === null?'':formContext.getAttribute("clp_holder").getValue()[0].name;
    const Title = "Acknowledge of ISMS Card(s)";
    doc.setFont("helvetica", "bold");
    doc.setFontSize(20);
    doc.text(Title, 60, 30);
    //! Basic Information
    doc.rect(20, 35, 180, 80);
    //* To
    doc.setFont("helvetica", "normal");
    doc.setFontSize(10);
    doc.text("To:", 25, 45);
    doc.text(ToName, 33, 45);
    //< span style = “text-decoration：underline;” >ToName< / span >
    doc.line(31, 45.5, doc.getTextWidth(ToName)+35, 45.5);

    //* No&&ReturnDate
    doc.text(`I have received the ISMS card No.   ${No}   on    ${ReturnDate}`, 25, 60);
    doc.line(98, 60.5, 80, 60.5);
    doc.line(130, 60.5, 104, 60.5);
    //* Signature
    doc.text("Signature:", 25, 75);
    doc.line(45, 75.5, 75, 75.5);
    //* Name in Block
    doc.text("Name in Block:", 25, 85);
    doc.line(50, 85.5, 75, 85.5);
    //* Dept
    doc.text("Dept.:", 25, 95);
    doc.line(35, 95.5, 65, 95.5);
    doc.save(`${Title}.pdf`)
}


PROXCardPrintAcknowledgementSDK.dateFormat = (fmt, date) => {
    let ret;
    const opt = {
        "Y+": date.getFullYear().toString(),        // 年
        "m+": (date.getMonth() + 1).toString(),     // 月
        "d+": date.getDate().toString(),            // 日
        "H+": date.getHours().toString(),           // 时
        "M+": date.getMinutes().toString(),         // 分
        "S+": date.getSeconds().toString()          // 秒
        // 有其他格式化字符需求可以继续添加，必须转化成字符串
    };
    for (let k in opt) {
        ret = new RegExp("(" + k + ")").exec(fmt);
        if (ret) {
            fmt = fmt.replace(ret[1], (ret[1].length == 1) ? (opt[k]) : (opt[k].padStart(ret[1].length, "0")))
        };
    };
    return fmt;
}

PROXCardPrintAcknowledgementSDK.enableButton = () => {
    const inputData = Xrm.Utility.getPageContext().input.data

    return ShareSdk.isProximityCardAdmin() && ShareSdk.IsProxCardReturnForm() && (inputData?.newCardStatus === ShareSdk.ProximityCardStatus.Returned || inputData.newCardStatus === ShareSdk.ProximityCardStatus.Damaged)
}