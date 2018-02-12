import * as React from 'react';

interface SoundProps {
  url: string;
  type: string;
}

interface SoundState {
  willPlay: boolean;
}

export class Sound extends React.Component<SoundProps, SoundState> {
  audioControl: HTMLAudioElement | null;

  constructor(props: SoundProps) {
    super(props);
    this.state = { willPlay: false };
  }

  componentWillReceiveProps(nextProps: SoundProps)
  {
    if (nextProps.url !== this.props.url)
    {
      this.setState({ willPlay: true });
    }
  }

  public render() {
    return <div>
      <audio src={this.props.url} type={this.props.type} ref={(audio) => { this.audioControl = audio; } } />
    </div>;
  }

  componentDidUpdate(prevProps: SoundProps, prevState: SoundState) {
    if (this.state.willPlay)
    {
      this.play();
      this.setState({ willPlay: false });
    }
  }

  play() {
    if (this.audioControl) {
      this.audioControl.play()
        .then(_ => {
          console.log("Playing...");
        })
        .catch(error => {
          console.log(error);
        });
    }
  }
}
