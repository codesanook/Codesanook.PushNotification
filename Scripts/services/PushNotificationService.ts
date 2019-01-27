import IConfig from "../models/IConfig";
import PushNoficationMessage from "../models/PushNotificationMessage";

//http://www.dwmkerr.com/promises-in-angularjs-the-definitive-guide/
export default class PushNotificationService {

    constructor(
        private $q: ng.IQService,
        private $http: ng.IHttpService,
        private config: IConfig
    ) {

    }

    private getApi<TResult>(url: string): ng.IPromise<TResult> {
        let method = "GET";
        let req = {
            method: method,
            url: url,
            headers: {
                'Content-Type': "application/json",
                'Accept': 'application/json'
            },
            data: {}
        };

        let deferred = this.$q.defer<TResult>();
        this.$http(req)
            .then(response => {
                //cast with as keyword
                let data = response.data as TResult;
                deferred.resolve(data);
            }).catch(error => {
                this.logError(url, error);
                deferred.reject(error);
            });
        return deferred.promise;
    }

    sendMessage(message: PushNoficationMessage): ng.IPromise<void> {
        const url = `${this.config.apiEndpoint}/PushNotifications`
        let method = "POST";
        let req = {
            method: method,
            url: url,
            headers: {
                'Content-Type': "application/json",
                'Accept': 'application/json'
            },
            data: message 
        };

        let deferred = this.$q.defer<void>();
        this.$http(req)
            .then(() => {
                deferred.resolve();
            }).catch(response => {
                deferred.reject(response.data.exceptionMessage);
            });
        return deferred.promise;
    }

    private logError(url: string, error): void {
        console.log(`API URL ${url} error ${JSON.stringify(error, null, 2)}`);
    }
}
