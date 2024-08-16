window.EngagementQuarterSDK = window.EngagementQuarterSDK || {}


EngagementQuarterSDK.onFormLoaded = () => {
    let date = new Date()
    let year = date.getFullYear()
    let name = Xrm.Page.data.entity.attributes.getByName("aia_name").getValue()

    if (!name) {
        Xrm.Page.data.entity.attributes.getByName("aia_name").setValue(year.toString())
        // window.parent.document.querySelector('h1[id*=formHeaderTitle]').innerText =  year.toString()
    }
}
EngagementQuarterSDK.reminderDateOnChange = () => {
    let controlName = "aia_set_reminder_on"
    formContext = Xrm.Page.ui.formContext
    formContext.getControl(controlName).clearNotification(EAAShareSdk.MessageUniqueIdMap.SetReminderOn);
    const date = Xrm.Page.data.entity.attributes.getByName(controlName).getValue()
    if (date) {
        const result = /^\d[\d,]+\d$/.test(date);
        if (!result) {
            formContext.getControl(controlName).addNotification({
                notificationLevel: 'ERROR',
                messages: [EAAShareSdk.ErrorMessages.SetReminderOn],
                uniqueId: EAAShareSdk.MessageUniqueIdMap.SetReminderOn
            });
        }
    }
}