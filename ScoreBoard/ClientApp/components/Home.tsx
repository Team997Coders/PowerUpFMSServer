import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { Game } from './Game';
import { Field } from './Field';
import { Reset } from './Reset';

interface HomeState {
  scoreboard: ScoreboardEnum
  message: string
}

enum ScoreboardEnum {
  Field,
  Reset,
  Game
}

export class Home extends React.Component<RouteComponentProps<{}>, HomeState> {
  serverURL: string;
  interval: number;

  constructor() {
    super();
    this.state = {
      scoreboard: ScoreboardEnum.Field,
      message: ""
    };
    this.serverURL = "http://192.168.1.54:5000";
  }

  // TODO: Ugh, very dense.
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

  componentWillMount() {
    this.interval = setInterval(this.updateScoreboardState.bind(this), 1000);
  }

  componentWillUnmount() {
    clearInterval(this.interval);
  }

  public render() {
    return <div>
      {this.state.scoreboard == ScoreboardEnum.Game &&
        <Game />
      }
      {this.state.scoreboard == ScoreboardEnum.Reset &&
        <Reset message={this.state.message} />
      }
      {this.state.scoreboard == ScoreboardEnum.Field &&
        <Field message={this.state.message} />
      }
    </div>
  }
}