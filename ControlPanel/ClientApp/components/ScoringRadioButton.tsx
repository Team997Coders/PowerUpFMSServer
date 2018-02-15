import * as React from 'react';
import { ChangeEvent } from 'react';

interface ICallback {
  ( newScore: number ) : void;
}

interface ScoringRadioButtonState {
  score: number;
}

interface ScoringRadioButtonProps {
  alliance: string;
  name: string
  callbackParent: ICallback;
  disabled: boolean
  maximumScore: number
}

export class ScoringRadioButton extends React.Component<ScoringRadioButtonProps, ScoringRadioButtonState> {
  constructor() {
      super();
      this.state = { score: 0 };
  }

  onScoreChanged(changeEvent: ChangeEvent<HTMLInputElement>) {
    const newScore = parseInt(changeEvent.target.value);
    this.setState({ score: newScore }); // we update our state
    this.props.callbackParent(newScore); // we notify our parent
  }

  isChecked(buttonValue : string) : boolean {
    return (parseInt(buttonValue) == this.state.score);
  }

  componentWillReceiveProps(nextProps: ScoringRadioButtonProps)
  {
    if (this.props.disabled != nextProps.disabled && nextProps.disabled == false)
    {
      this.setState({ score: 0 });
    }
  }

  public render() {
    var buttons = 
    Array.apply(null, {length: this.props.maximumScore + 1}).map(Function.call, Number).map((i: number) =>
      <label key={i}>
        <input 
          type="radio" 
          name={this.props.name} 
          value={i.toString()} 
          checked={this.isChecked(i.toString())}
          disabled={this.props.disabled}
          onChange={e => this.onScoreChanged(e)} />
        {i}&nbsp;&nbsp;
      </label>
    );
    
    return <div> 
      <div>{this.props.alliance}:&nbsp;</div>
      <div className="radio">
        {buttons}
      </div>
    </div>;
  }
}