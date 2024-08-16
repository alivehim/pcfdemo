var PROXCardAuditReassignSDK = window.PROXCardAuditReassignSDK || {};

PROXCardAuditReassignSDK.onAction = async (ids, selectControl) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {


        const selectIds = ids;

        let isAudit = false;
        //* for batch select update
        for (let i = 0; i < selectIds.length; i++) {
            const e = selectIds[i];
            const curId = e.replace(/[\{\}]*/g, "").toLowerCase();
            const curAudit = await ShareSdk.getAduitDetailAsync(curId);

            //* Check record is audited. Reassign Page Id: <none>
            if (curAudit.clp_isaudited) {
                isAudit = true;
            }
        }

        //* Check all records had audited.
        if (isAudit) {
            //MKCCardAuditSDK.NotificationFunc("Confirm", "The record has been audited, please confirm the record you want to reassign.", "Notification", 120, 260);

            var confirmStrings = { text: "The record has been audited, please confirm the record you want to reassign.", title: "Notification" };
            var confirmOptions = { height: 200, width: 450 };
            Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
                function (success) {

                });

        }
        //* Navigate func.
        else {
            const pageInput = {
                pageType: "custom",
                name: "clp_ssmspcauditdetailreassign_b194c",
                entityName: "clp_proximity_card_audit_detail",
                recordId: selectIds.toString()
            };
            const navigationOptions = {
                target: 2,
                height: { value: 50, unit: "%" },
                width: { value: 40, unit: "%" },
                position: 1
            };

            await Xrm.Navigation.navigateTo(pageInput, navigationOptions)

            var auditId = ShareSdk.getCurrentFormId()
            var audits = await ShareSdk.getAduitAsync(auditId)
            if (audits.length!=0) {
                selectControl.refresh()
            } else {


                var listInput = {
                    pageType: "entitylist",
                    entityName: "clp_proximity_card_audit",
                }

                Xrm.Navigation.navigateTo(listInput)


            }

        }


    } catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}




PROXCardAuditReassignSDK.enableButton = () => {
    return ShareSdk.isProximityCardAdmin()
}

