import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Game } from './Game';
import { Field } from './Field';
import { Reset } from './Reset';

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
  scoreboard: ScoreboardEnum
  message: string
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
  stateOfPlay: string;
}

enum ScoreboardEnum {
  Ready,
  Reset,
  Game
}

export class Home extends React.Component<RouteComponentProps<{}>, HomeState> {
  serverURL: string;
//  interval: number;
  eventSource: EventSource;

  constructor() {
    super();
    this.state = {
      scoreboard: ScoreboardEnum.Game,
      message: "",
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
      stateOfPlay: ""
    };
    this.serverURL = "http://192.168.1.54:5000";
  }

  // TODO: Ugh, very dense.
/*
  updateScoreboardState() {
    fetch(`${this.serverURL}/api/field/state`)
    .then((response) => response.text())
    .then((responseText) => {
      if (responseText == "GAME") {
        fetch(`${this.serverURL}/api/game/state`)
        .then((response) => response.text())
        .then((responseText) => {
          if (responseText == "RESET") {
            this.setState({ scoreboard: ScoreboardEnum.Reset} );
            fetch(`${this.serverURL}/api/field/fieldstring`)
            .then((response) => response.text())
            .then((responseText) => {
              this.setState({ message: "Field String: " + responseText});
            });
          } else {
            this.setState({ scoreboard: ScoreboardEnum.Game} );
          }
        });
      } else {
        this.setState( { scoreboard: ScoreboardEnum.Field} );
        if (responseText == "OFF") {
          this.setState( {message: "NO ENTRY"} );
        } else if (responseText == "SAFE") {
          this.setState( {message: "SAFE"} );
        } else if (responseText == "STAFFSAFE") {
          this.setState( {message: "STAFF SAFE"} );
        } else {
          this.setState( {message: "UNKNOWN"} );          
        }
      }
    });
  } 
*/

  componentWillMount() {
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
        self.setState({ stateOfPlay: message['StateOfPlay'] });
        if (message['StateOfPlay'] == "RESET")
        {
          self.setState({ 
            redScaleSecs: 0,
            redSwitchSecs: 0,
            redTotalSecs: 0,
            redVaultScore: 0,
            redParkScore: 0,
            redAutorunScore: 0,
            redClimbScore: 0,
            blueScaleSecs: 0,
            blueSwitchSecs: 0,
            blueTotalSecs: 0,
            blueVaultScore: 0,
            blueParkScore: 0,
            blueAutorunScore: 0,
            blueClimbScore: 0,
            gameClock: 0,
            scoreboard: ScoreboardEnum.Reset
          });
          fetch(`${self.serverURL}/api/field/fieldstring`)
            .then((response) => response.text())
            .then((responseText) => {
              self.setState({ message: "Field String: " + responseText});
            });
      }
        else if (message['StateOfPlay'] == "DRIVERCOUNTDOWN")
        {
          self.setState({ 
            scoreboard: ScoreboardEnum.Ready,
            message: "Ready!"          
          });
        }
        else if (message['StateOfPlay'] == "AUTONOMOUSCOUNTDOWN")
        {
          self.setState({
            scoreboard: ScoreboardEnum.Game
          });
        }
        return;  
      }
    }
  }

  componentWillUnmount() {
    this.eventSource.close();
  }

//  componentWillMount() {
//    this.interval = setInterval(this.updateScoreboardState.bind(this), 1000);
//  }

//  componentWillUnmount() {
//    clearInterval(this.interval);
//  }

  public render() {
    return <div>
      {this.state.scoreboard == ScoreboardEnum.Game &&
        <Game 
          gameClock={this.state.gameClock}
          redSwitchSecs={this.state.redSwitchSecs}
          redScaleSecs={this.state.redScaleSecs}
          redTotalSecs={this.state.redTotalSecs}
          redVaultScore={this.state.redVaultScore}
          redParkScore={this.state.redParkScore}
          redAutorunScore={this.state.redAutorunScore}
          redClimbScore={this.state.redClimbScore}
          blueTotalSecs={this.state.blueTotalSecs}
          blueSwitchSecs={this.state.blueSwitchSecs}
          blueScaleSecs={this.state.blueScaleSecs}
          blueVaultScore={this.state.blueVaultScore}
          blueParkScore={this.state.blueParkScore}
          blueAutorunScore={this.state.blueAutorunScore}
          blueClimbScore={this.state.blueClimbScore}
          stateOfPlay={this.state.stateOfPlay}
          serverURL={this.serverURL}
        />
      }
      {this.state.scoreboard == ScoreboardEnum.Reset &&
        <Reset message={this.state.message} />
      }
      {this.state.scoreboard == ScoreboardEnum.Ready &&
        <Field message={this.state.message} />
      }
    </div>
  }
}