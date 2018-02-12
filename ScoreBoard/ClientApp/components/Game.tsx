import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { TotalScore } from './TotalScore';
import { Timer } from './Timer';
import { Breakdown } from './Breakdown';
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
  blueTotalSecs: number;
  blueSwitchSecs: number;
  blueScaleSecs: number;
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
      blueSwitchSecs: 0,
      blueScaleSecs: 0,
      blueTotalSecs: 0,
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
          self.setState({ blueScaleSecs: 0 });
          self.setState({ blueSwitchSecs: 0 });
          self.setState({ blueTotalSecs: 0 });
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
        <Breakdown alliance='red' switch={this.state.redSwitchSecs} scale={this.state.redScaleSecs} climb={0} />
        <Breakdown alliance='#4169E1' switch={this.state.blueSwitchSecs} scale={this.state.blueScaleSecs} climb={0} />
      </div>
      <Sound url={this.state.playFile} type="audio/mpeg" />
    </div>
  }
}
