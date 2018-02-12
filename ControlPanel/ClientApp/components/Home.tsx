import * as React from 'react';
import { RouteComponentProps } from 'react-router';

// These definitions are missing from VSCODE at this time...arg!
interface EventSource extends EventTarget {
    readonly url: string;
    readonly withCredentials: boolean;
    readonly CONNECTING: number;
    readonly OPEN: number;
    readonly CLOSED: number;
    readonly readyState: number;
    onopen: (evt: MessageEvent) => any;
    onmessage: (evt: MessageEvent) => any;
    onerror: (evt: MessageEvent) => any;
    close(): void;
}

declare var EventSource: {
    prototype: EventSource;
    new(url: string, eventSourceInitDict?: EventSourceInit): EventSource;
};

interface EventSourceInit {
    readonly withCredentials: boolean;
}

interface HomeState {
  playing: boolean,
  reset: boolean,
  gameClock: number,
  redSwitchSecs: number,
  redScaleSecs: number,
  redTotalSecs: number,
  blueSwitchSecs: number,
  blueScaleSecs: number,
  blueTotalSecs: number
}

export class Home extends React.Component<RouteComponentProps<{}>, HomeState> {
  serverURL: string;
  fetchParameters = {};
  eventSource: EventSource;
  
  constructor() {
    super();
    this.state = {
      playing: false, 
      reset: false, 
      gameClock: 0,
      redSwitchSecs: 0,
      redScaleSecs: 0,
      redTotalSecs: 0,
      blueSwitchSecs: 0,
      blueScaleSecs: 0,
      blueTotalSecs:0
    };
    this.serverURL = "http://192.168.1.54:5000";
    this.fetchParameters = { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded'} };
  }

  public render() {
      return <div>
        <h1>Field Control</h1>
        <button disabled={this.state.playing} onClick={ () => { this.fieldOff() } }>Off</button>
        <button disabled={this.state.playing} onClick={ () => { this.safe() } }>Safe</button>
        <button disabled={this.state.playing} onClick={ () => { this.staffSafe() } }>Staff Safe</button>
        <h1>Game Control</h1>
        <button disabled={this.state.playing} onClick={ () => { this.reset() } }>Reset</button>
        <button disabled={this.state.playing || !this.state.reset} onClick={ () => { this.play() } }>Play</button>
        <button disabled={!this.state.playing} onClick={ () => { this.fault() } }>Fault</button>
        <h1>Game Status</h1>
        {this.state.playing && <p><strong>PLAYING</strong></p>}
        <p>Game clock: <strong>{ this.state.gameClock }</strong></p>
        <p>Red switch seconds: <strong>{ this.state.redSwitchSecs }</strong></p>
        <p>Red scale seconds: <strong>{ this.state.redScaleSecs }</strong></p>
        <p>Red total seconds: <strong>{ this.state.redTotalSecs }</strong></p>
        <p>Blue switch seconds: <strong>{ this.state.blueSwitchSecs }</strong></p>
        <p>Blue scale seconds: <strong>{ this.state.blueScaleSecs }</strong></p>
        <p>Blue total seconds: <strong>{ this.state.blueTotalSecs }</strong></p>
      </div>;
  }

  fieldOff() {
    fetch(`${this.serverURL}/api/Field/Off`, this.fetchParameters);
    this.setState({ reset: false });
  }

  safe() {
    fetch(`${this.serverURL}/api/Field/Safe`, this.fetchParameters);
    this.setState({ reset: false });
  }

  staffSafe() {
    fetch(`${this.serverURL}/api/Field/StaffSafe`, this.fetchParameters);
    this.setState({ reset: false });
  }

  play() {
    fetch(`${this.serverURL}/api/Game/Play`, this.fetchParameters);
    this.setState({ playing: true });
    this.setState({ reset: false });
    let self = this;
    this.eventSource = new EventSource(`${this.serverURL}/api/Game`);
    this.eventSource.onmessage = function(e) {
      let message = JSON.parse(e.data);
      if ("ElapsedSeconds" in message)
      {
        self.setState({ gameClock: message['ElapsedSeconds'] });
        return;
      }
      if ("BlueOwnershipSeconds" in message)
      {
        self.setState({ blueTotalSecs: message['BlueOwnershipSeconds'] });
        return;
      }
      if ("RedOwnershipSeconds" in message)
      {
        self.setState({ redTotalSecs: message['RedOwnershipSeconds'] });
        return;
      }
      if ("BlueSwitchOwnershipSeconds" in message)
      {
        self.setState({ blueSwitchSecs: message['BlueSwitchOwnershipSeconds'] });
        return;
      }
      if ("BlueScaleOwnershipSeconds" in message)
      {
        self.setState({ blueScaleSecs: message['BlueScaleOwnershipSeconds'] });
        return;
      }
      if ("RedSwitchOwnershipSeconds" in message)
      {
        self.setState({ redSwitchSecs: message['RedSwitchOwnershipSeconds'] });
        return;
      }
      if ("RedScaleOwnershipSeconds" in message)
      {
        self.setState({ redScaleSecs: message['RedScaleOwnershipSeconds'] });
        return;
      }
      if ("EndGame" in message)
      {
        self.eventSource.close();
        self.setState({ playing: false });
      }
    }
  }

  reset() {
    fetch(`${this.serverURL}/api/Game/Reset`, this.fetchParameters);
    this.setState({ reset: true });
    this.setState({
      gameClock: 0,
      redSwitchSecs: 0,
      redScaleSecs: 0,
      redTotalSecs: 0,
      blueSwitchSecs: 0,
      blueScaleSecs: 0,
      blueTotalSecs:0      
    });
  }

  fault() {
    this.eventSource.close();
    fetch(`${this.serverURL}/api/Game/Fault`, this.fetchParameters)
      .catch(error => console.log(error));
    this.setState({ playing: false });
  }
}
