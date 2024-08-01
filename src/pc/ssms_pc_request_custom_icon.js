function displayIconTooltip(rowData, userLCID) {

    var str = JSON.parse(rowData);
    var coldata = str.clp_attachment_link;


    if (coldata) {
        var imageName = 'clp_ssms_icon_attach.svg'
        var resultarray = [imageName,'file'];
        return resultarray;
    } else {
        return []
    }

}  