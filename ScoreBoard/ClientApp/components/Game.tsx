import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { TotalScore } from './TotalScore';
import { Timer } from './Timer';
import { BreakdownL1 } from './BreakdownL1';
import { BreakdownL2 } from './BreakdownL2';
import { Sound } from './Sound';

//const Sound = require('react-sound');

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

interface GameState {
  gameClock: number;
  redSwitchSecs: number;
  redScaleSecs: number;
  redTotalSecs: number;
  redVaultScore: number;
  redParkScore: number;
  redAutorunScore: number;
  redClimbScore: number;
  blueTotalSecs: number;
  blueSwitchSecs: number;
  blueScaleSecs: number;
  blueVaultScore: number;
  blueParkScore: number;
  blueAutorunScore: number;
  blueClimbScore: number;
  playFile: string;
}

export class Game extends React.Component<{}, GameState> {
  serverURL: string;
  fetchParameters = {};
  eventSource: EventSource;

  constructor() {
    super();
    this.serverURL = "http://192.168.1.54:5000";
    this.state = {
      gameClock: 0,
      redSwitchSecs: 0,
      redScaleSecs: 0,
      redTotalSecs: 0,
      redVaultScore: 0,
      redParkScore: 0,
      redAutorunScore: 0,
      redClimbScore: 0,
      blueSwitchSecs: 0,
      blueScaleSecs: 0,
      blueTotalSecs: 0,
      blueVaultScore: 0,
      blueParkScore: 0,
      blueAutorunScore: 0,
      blueClimbScore: 0,
      playFile: ""
    };
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
      if ("RedVaultScore" in message)
      {
        self.setState({ redVaultScore: message['RedVaultScore'] });
        return;
      }
      if ("BlueVaultScore" in message)
      {
        self.setState({ blueVaultScore: message['BlueVaultScore'] });
        return;
      }
      if ("RedParkScore" in message)
      {
        self.setState({ redParkScore: message['RedParkScore'] });
        return;
      }
      if ("BlueParkScore" in message)
      {
        self.setState({ blueParkScore: message['BlueParkScore'] });
        return;
      }
      if ("RedAutorunScore" in message)
      {
        self.setState({ redAutorunScore: message['RedAutorunScore'] });
        return;
      }
      if ("BlueAutorunScore" in message)
      {
        self.setState({ blueAutorunScore: message['BlueAutorunScore'] });
        return;
      }
      if ("RedClimbScore" in message)
      {
        self.setState({ redClimbScore: message['RedClimbScore'] });
        return;
      }
      if ("BlueClimbScore" in message)
      {
        self.setState({ blueClimbScore: message['BlueClimbScore'] });
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
      if ("StateOfPlay" in message)
      {
        if (message['StateOfPlay'] == "AUTONOMOUS")
        {
          self.setState({ playFile: "/audio/auto.mp3" });          
        }
        else if (message['StateOfPlay'] == "TELEOPERATED")
        {
          self.setState({ playFile: "/audio/teleop.mp3" });
        }
        else if (message['StateOfPlay'] == "ENDGAME")
        {
          self.setState({ playFile: "/audio/endgame.mp3" });
        }
        else if (message['StateOfPlay'] == "GAMEOVER")
        {
          self.setState({ playFile: "/audio/matchover.mp3" });
        }
        else if (message['StateOfPlay'] == "FAULTED")
        {
          self.setState({ playFile: "/audio/fault.mp3" });
        }
        else if (message['StateOfPlay'] == "RESET")
        {
          self.setState({ playFile: "" });
          self.setState({ redScaleSecs: 0 });
          self.setState({ redSwitchSecs: 0 });
          self.setState({ redTotalSecs: 0 });
          self.setState({ redVaultScore: 0 });
          self.setState({ redParkScore: 0 });
          self.setState({ redAutorunScore: 0 });
          self.setState({ redClimbScore: 0 });
          self.setState({ blueScaleSecs: 0 });
          self.setState({ blueSwitchSecs: 0 });
          self.setState({ blueTotalSecs: 0 });
          self.setState({ blueVaultScore: 0 });
          self.setState({ blueParkScore: 0 });
          self.setState({ blueAutorunScore: 0 });
          self.setState({ blueClimbScore: 0 });
          self.setState({ gameClock: 0 });
        }
        return;
      }
    }
  }

  public render() {
    return <div>
      <div className='row'>
        <div className='col-md-12 text-center'>
          <Timer seconds={this.state.gameClock}/>
        </div>
      </div>
      <div className='row'>
        <div className='col-md-6 text-center'>
          <TotalScore alliance='red' score={this.state.redTotalSecs} />
        </div>
        <div className='col-md-6 text-center'>
          <TotalScore alliance='#4169E1' score={this.state.blueTotalSecs}/>
        </div>
      </div>
      <div className='row'>
        <BreakdownL1 alliance='red' switch={this.state.redSwitchSecs} scale={this.state.redScaleSecs} climb={this.state.redClimbScore} />
        <BreakdownL1 alliance='#4169E1' switch={this.state.blueSwitchSecs} scale={this.state.blueScaleSecs} climb={this.state.blueClimbScore} />
      </div>
      <div className='row'>
        <BreakdownL2 alliance='red' vault={this.state.redVaultScore} park={this.state.redParkScore} autorun={this.state.redAutorunScore} />
        <BreakdownL2 alliance='#4169E1' vault={this.state.blueVaultScore} park={this.state.blueParkScore} autorun={this.state.blueAutorunScore} />
      </div>
      <Sound url={this.state.playFile} type="audio/mpeg" />
    </div>
  }
}
