
export interface ViewerImageSize {
  width: number;
  height: number;
}


export interface ImageDecorator {
  src: string;
  alt: string;
  downloadUrl?: string;
  defaultSize?: ViewerImageSize;
}

export interface ToolbarConfig {
  key: string;
  actionType: number;
  render?: React.ReactNode;
  onClick?: (activeImage: ImageDecorator) => void;
}