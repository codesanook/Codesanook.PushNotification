import PushNotificationMessage from "../models/PushNotificationMessage";
import PushNotificationService from "../services/PushNotificationService";

export default class HospitalController {

    message: PushNotificationMessage = new PushNotificationMessage();

    constructor(
        private $scope: any,
        private $window: ng.IWindowService,
        private pushNotificationService: PushNotificationService
    ) {
        console.log(`PushNotificationControlller loaded`);
    }

    submitForm(form: any, $event: any): void {
        if (!form.$valid) {
            form.$submitted = true;
            $event.preventDefault();
            return;
        }

        this.sendPushNotification();
    }

    sendPushNotification(): void {
        this.pushNotificationService
            .sendMessage(this.message)
            .then(() => {
                alert('sent');
            }).catch(error => {
                alert(`${JSON.stringify(error, null, 2)}`);
            });
    }
}
