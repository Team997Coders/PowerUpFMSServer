import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { TotalScore } from './TotalScore';
import { Timer } from './Timer';
import { BreakdownL1 } from './BreakdownL1';
import { BreakdownL2 } from './BreakdownL2';
import { Sound } from './Sound';

interface GameProps {
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
  serverURL: string;
}

export class Game extends React.Component<GameProps, {}> {

  constructor(props: GameProps) {
    super(props);
  }

  public render() {
    let playFile = "";
    if (this.props.stateOfPlay == "DRIVERCOUNTDOWN")
    {
      playFile = "/audio/ready.mp3";          
    }
    else if (this.props.stateOfPlay == "AUTONOMOUS")
    {
      playFile = "/audio/auto.mp3";          
    }
    else if (this.props.stateOfPlay == "TELEOPERATED")
    {
      playFile = "/audio/teleop.mp3";
    }
    else if (this.props.stateOfPlay == "ENDGAME")
    {
      playFile = "/audio/endgame.mp3";
    }
    else if (this.props.stateOfPlay == "GAMEOVER")
    {
      playFile = "/audio/matchover.mp3";
    }
    else if (this.props.stateOfPlay == "FAULTED")
    {
      playFile = "/audio/fault.mp3";
    }

    return <div>
      <div className='row'>
        <div className='col-md-12 text-center'>
          <Timer seconds={this.props.gameClock}/>
        </div>
      </div>
      <div className='row'>
        <div className='col-md-6 text-center'>
          <TotalScore alliance='red' score={this.props.redTotalSecs} />
        </div>
        <div className='col-md-6 text-center'>
          <TotalScore alliance='#4169E1' score={this.props.blueTotalSecs}/>
        </div>
      </div>
      <div className='row'>
        <BreakdownL1 alliance='red' switch={this.props.redSwitchSecs} scale={this.props.redScaleSecs} climb={this.props.redClimbScore} />
        <BreakdownL1 alliance='#4169E1' switch={this.props.blueSwitchSecs} scale={this.props.blueScaleSecs} climb={this.props.blueClimbScore} />
      </div>
      <div className='row'>
        <BreakdownL2 alliance='red' vault={this.props.redVaultScore} park={this.props.redParkScore} autorun={this.props.redAutorunScore} />
        <BreakdownL2 alliance='#4169E1' vault={this.props.blueVaultScore} park={this.props.blueParkScore} autorun={this.props.blueAutorunScore} />
      </div>
      <Sound url={playFile} type="audio/mpeg" />
    </div>
  }
}
