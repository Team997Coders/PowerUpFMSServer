import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { ScoringRadioButton } from './ScoringRadioButton';


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
    this.fetchParameters = { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'Connection': 'close'} };
    this.eventSource = new EventSource(`${this.serverURL}/api/Game`);
    let self = this;
    self.eventSource.onmessage = function(e) {
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
        self.setState({ playing: false });
      }
    }
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
        <h1>Game Scoring</h1>
        <h2>Auto-Run</h2>
        <ScoringRadioButton alliance="Red" name="redautorun" disabled={!this.state.playing} maximumScore={3} callbackParent={s => this.onRedAutoRunScoreChanged(s)}/>
        <ScoringRadioButton alliance="Blue" name="blueautorun" disabled={!this.state.playing} maximumScore={3} callbackParent={s => this.onBlueAutoRunScoreChanged(s)}/>
        <h2>Vault</h2>
        <ScoringRadioButton alliance="Red" name="redvault" disabled={!this.state.playing} maximumScore={9} callbackParent={s => this.onRedVaultScoreChanged(s)}/>
        <ScoringRadioButton alliance="Blue" name="bluevault" disabled={!this.state.playing} maximumScore={9} callbackParent={s => this.onBlueVaultScoreChanged(s)}/>
        <h2>Park</h2>
        <ScoringRadioButton alliance="Red" name="redpark" disabled={!this.state.playing} maximumScore={3} callbackParent={s => this.onRedParkScoreChanged(s)}/>
        <ScoringRadioButton alliance="Blue" name="bluepark" disabled={!this.state.playing} maximumScore={3} callbackParent={s => this.onBlueParkScoreChanged(s)}/>
        <h2>Climb</h2>
        <ScoringRadioButton alliance="Red" name="redclimb" disabled={!this.state.playing} maximumScore={3} callbackParent={s => this.onRedClimbScoreChanged(s)}/>
        <ScoringRadioButton alliance="Blue" name="blueclimb" disabled={!this.state.playing} maximumScore={3} callbackParent={s => this.onBlueClimbScoreChanged(s)}/>
        <h1>Game Status</h1>
        {this.state.playing && <p><strong>PLAYING</strong></p>}
        <p>Game clock: <strong>{ this.state.gameClock }</strong></p>
        <p>Red switch seconds: <strong>{ this.state.redSwitchSecs }</strong></p>
        <p>Red scale seconds: <strong>{ this.state.redScaleSecs }</strong></p>
        <p>Red total: <strong>{ this.state.redTotalSecs }</strong></p>
        <p>Blue switch seconds: <strong>{ this.state.blueSwitchSecs }</strong></p>
        <p>Blue scale seconds: <strong>{ this.state.blueScaleSecs }</strong></p>
        <p>Blue total: <strong>{ this.state.blueTotalSecs }</strong></p>
      </div>;
  }

  onRedAutoRunScoreChanged(newScore: number) {
    this.postData(`${this.serverURL}/api/game/autorun`, { alliance: 'Red', score: newScore.toString()});
  }

  onBlueAutoRunScoreChanged(newScore: number) {
    this.postData(`${this.serverURL}/api/game/autorun`, { alliance: 'Blue', score: newScore.toString()});
  }

  onRedVaultScoreChanged(newScore: number) {
    this.postData(`${this.serverURL}/api/game/vault`, { alliance: 'Red', score: newScore.toString()});
  }

  onBlueVaultScoreChanged(newScore: number) {
    this.postData(`${this.serverURL}/api/game/vault`, { alliance: 'Blue', score: newScore.toString()});
  }

  onRedParkScoreChanged(newScore: number) {
    this.postData(`${this.serverURL}/api/game/park`, { alliance: 'Red', score: newScore.toString()});
  }

  onBlueParkScoreChanged(newScore: number) {
    this.postData(`${this.serverURL}/api/game/park`, { alliance: 'Blue', score: newScore.toString()});
  }

  onRedClimbScoreChanged(newScore: number) {
    this.postData(`${this.serverURL}/api/game/climb`, { alliance: 'Red', score: newScore.toString()});
  }

  onBlueClimbScoreChanged(newScore: number) {
    this.postData(`${this.serverURL}/api/game/climb`, { alliance: 'Blue', score: newScore.toString()});
  }

  fieldOff() {
    let self = this;
    fetch(`${this.serverURL}/api/Field/Off`, this.fetchParameters).then(function(response) {
      self.setState({ reset: false });
    });
  }

  safe() {
    let self = this;
    fetch(`${this.serverURL}/api/Field/Safe`, this.fetchParameters).then(function(response) {
      self.setState({ reset: false });
    });
  }

  staffSafe() {
    let self = this;
    fetch(`${this.serverURL}/api/Field/StaffSafe`, this.fetchParameters).then(function(response) {
      self.setState({ reset: false });
    });
  }

  play() {
    let self = this;
    fetch(`${this.serverURL}/api/Game/state/Play`, this.fetchParameters).then(function(response) {
      self.setState({ playing: true });
      self.setState({ reset: false });
    });
  }

  reset() {
    let self = this;
    fetch(`${this.serverURL}/api/Game/state/Reset`, this.fetchParameters).then(function(response) {
      self.setState({ reset: true });
      self.setState({
        gameClock: 0,
        redSwitchSecs: 0,
        redScaleSecs: 0,
        redTotalSecs: 0,
        blueSwitchSecs: 0,
        blueScaleSecs: 0,
        blueTotalSecs:0      
      });
    });
  }

  fault() {
    // this.eventSource.close();
    fetch(`${this.serverURL}/api/Game/state/Fault`, this.fetchParameters)
      .then(function(response) { })
      .catch(error => console.log(error));
    // this.setState({ playing: false });
  }

  urlEncodeBody(data: { [key: string]: string }) : string {
    var str = [];
    for(var p in data)
      str.push(encodeURIComponent(p) + "=" + encodeURIComponent(data[p]));
    return str.join("&");
  }

  postData(url: string, data: { [key: string]: string } ) {
    // Default options are marked with *
    return fetch(url, {
      body: this.urlEncodeBody(data),
      cache: 'no-cache', // *default, cache, reload, force-cache, only-if-cached
      credentials: 'same-origin', // include, *omit
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
        'Connection': 'close'
      },
      method: 'POST', // *GET, PUT, DELETE, etc.
      mode: 'cors', // no-cors, *same-origin
      redirect: 'follow', // *manual, error
      referrer: 'no-referrer', // *client
    })
    .then(response => response.toString()) // simply returns the response
  }  
}
