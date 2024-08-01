
var PROXCardCreateFormSDK = window.PROXCardCreateFormSDK || {};


PROXCardCreateFormSDK.cardNoReg = /^[A-Za-z0-9]{2}[0-9]{5}$/;
PROXCardCreateFormSDK.onFormLoaded = () => {
    // formContext = Xrm.Page.ui.formContext
    // var currentForm = formContext.ui.formSelector.getCurrentItem();

    // if (currentForm.getId() != ShareSdk.Forms.Card_Create) {
    //     formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Card_Create);
    //     formItem.navigate();
    // }
    //Xrm.UI._context.Router._preNavigationHandlers = []
}

PROXCardCreateFormSDK.card_no_fromChanged = () => {
    formContext = Xrm.Page.ui.formContext
    var card_no_from = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardCreationFields.card_no_from).getValue()
    var card_no_to = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardCreationFields.card_no_to).getValue()
    formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_from).clearNotification(ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.id);
    formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_from).clearNotification(ShareSdk.PROXCardInventoryErrors.ISMSProximityCardDeptConsistant.id);
    formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_from).clearNotification(ShareSdk.PROXCardInventoryErrors.ISMSProximityCardRange.id);


    if (card_no_from) {

        if (!PROXCardCreateFormSDK.cardNoReg.test(card_no_from)) {
            formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_from).addNotification({
                notificationLevel: 'ERROR',
                messages: [ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.message],
                uniqueId: ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.id
            });
            return
        }

        if (card_no_to) {
            var deptnofrom = card_no_from.substring(0, 2)
            var deptnoto = card_no_to.substring(0, 2)

            if (deptnofrom != deptnoto) {
                formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_from).addNotification({
                    notificationLevel: 'ERROR',
                    messages: [ShareSdk.PROXCardInventoryErrors.ISMSProximityCardDeptConsistant.message],
                    uniqueId: ShareSdk.PROXCardInventoryErrors.ISMSProximityCardDeptConsistant.id
                });
                return
            }

            var snofrom = card_no_from.substring(2, 8)
            var snoto = card_no_to.substring(2, 8)

            if (snoto <= snofrom) {
                formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_from).addNotification({
                    notificationLevel: 'ERROR',
                    messages: [ShareSdk.PROXCardInventoryErrors.ISMSProximityCardRange.message],
                    uniqueId: ShareSdk.PROXCardInventoryErrors.ISMSProximityCardRange.id
                });
                return
            }
        }
    }
}

PROXCardCreateFormSDK.card_no_toChanged = () => {
    formContext = Xrm.Page.ui.formContext
    var card_no_to = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardCreationFields.card_no_to).getValue()

    var card_no_from = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardCreationFields.card_no_from).getValue()

    formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_to).clearNotification(ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.id);
    formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_to).clearNotification(ShareSdk.PROXCardInventoryErrors.ISMSProximityCardDeptConsistant.id);
    formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_to).clearNotification(ShareSdk.PROXCardInventoryErrors.ISMSProximityCardRange.id);


    if (card_no_from) {

        if (!PROXCardCreateFormSDK.cardNoReg.test(card_no_to)) {
            formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_to).addNotification({
                notificationLevel: 'ERROR',
                messages: [ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.message],
                uniqueId: ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.id
            });
            return
        }

        if (card_no_from) {
            var deptnofrom = card_no_from.substring(0, 2)
            var deptnoto = card_no_to.substring(0, 2)

            if (deptnofrom != deptnoto) {
                formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_to).addNotification({
                    notificationLevel: 'ERROR',
                    messages: [ShareSdk.PROXCardInventoryErrors.ISMSProximityCardDeptConsistant.message],
                    uniqueId: ShareSdk.PROXCardInventoryErrors.ISMSProximityCardDeptConsistant.id
                });
                return
            }

            var snofrom = card_no_from.substring(2, 8)
            var snoto = card_no_to.substring(2, 8)

            if (snoto <= snofrom) {
                formContext.getControl(ShareSdk.PROXCardCreationFields.card_no_to).addNotification({
                    notificationLevel: 'ERROR',
                    messages: [ShareSdk.PROXCardInventoryErrors.ISMSProximityCardRange.message],
                    uniqueId: ShareSdk.PROXCardInventoryErrors.ISMSProximityCardRange.id
                });
                return
            }
        }
    }
}