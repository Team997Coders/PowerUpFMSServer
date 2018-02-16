import * as React from 'react';
import { RouteComponentProps } from 'react-router';

interface TotalScoreProps {
  alliance: string;
  score: number;
}

export class TotalScore extends React.Component<TotalScoreProps, {}> {
  constructor(props: TotalScoreProps) {
    super(props);
  }

  public render() {
    var styles = {
      color: this.props.alliance,
      fontSize: 210,
      lineHeight: '1.50em',
      fontFamily: "'Press Start 2P'",
    }

    return <div style={styles}>
      {this.props.score}
    </div>;
  }
}
