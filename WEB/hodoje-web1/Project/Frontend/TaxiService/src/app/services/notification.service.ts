import { Injectable } from '@angular/core';

declare var $: any;
@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private connection: any;
  private proxy: any;

  constructor() {
    this.connection = $.hubConnection('http://localhost:3737');
    this.proxy = this.connection.createHubProxy('rideStatusUpdateHub');
    this.proxy.on('messageReceived', (latestMsg) => this.onMessageReceived(latestMsg));
    this.startConnection();
  }

  private startConnection(): void {  
    this.connection.start().done((data: any) => {  
        console.log('Now connected ' + data.transport.name + ', connection ID = ' + data.id);  
    }).fail((error: any) => {  
        console.log('Could not connect ' + error);  
    });  
  } 
  
  private onMessageReceived(latestMsg: string){
    console.log('New message recieved: ' + latestMsg);
  }

  broadcastMessage(msg: string){
    this.proxy.invoke('sendMessage', msg);
  }
}
