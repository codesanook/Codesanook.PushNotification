import PushNotificationController from "./controllers/PushNotificationController"
import IConfig from "./models/IConfig";
import PushNotificationService from "./services/PushNotificationService";

let app = angular.module("pushNotification", [
    'ngMessages'  //To use angular ngMessage for form validation;
]);
   
let config: IConfig = {
    apiEndpoint : '/api/CodeSanook.PushNotification'
};

//inject configuration
app.constant("config", config);

//register controllers and services
app.controller("pushNotificationController", PushNotificationController); 
app.service("pushNotificationService", PushNotificationService); 

//binding to element after module is ready
let rootElement = angular.element(".push-notification-app").get(0);
angular.bootstrap(rootElement, ['pushNotification']);
