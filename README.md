# README - GhostOnly

### 목차

1. [**게임 소개**](https://github.com/jungbosong/GhostOnly#1-%EA%B2%8C%EC%9E%84-%EC%86%8C%EA%B0%9C)
2. [**팀원 소개 및 개발 기간**](https://github.com/jungbosong/GhostOnly#2-%ED%8C%80%EC%9B%90-%EC%86%8C%EA%B0%9C-%EB%B0%8F-%EA%B0%9C%EB%B0%9C-%EA%B8%B0%EA%B0%84-)
3. [**기능별 클래스 설명**](https://github.com/jungbosong/GhostOnly#3-%EA%B8%B0%EB%8A%A5%EB%B3%84-%ED%81%B4%EB%9E%98%EC%8A%A4-%EC%84%A4%EB%AA%85-)
4. [**사용한 기술 목록**](https://github.com/jungbosong/GhostOnly#4-%EC%82%AC%EC%9A%A9%ED%95%9C-%EA%B8%B0%EC%88%A0-%EB%AA%A9%EB%A1%9D-)
5. [**트러블 슈팅**](https://github.com/jungbosong/GhostOnly#5-%ED%8A%B8%EB%9F%AC%EB%B8%94-%EC%8A%88%ED%8C%85-)
6. [**라이선스**](https://github.com/jungbosong/GhostOnly#6-%EB%9D%BC%EC%9D%B4%EC%84%A0%EC%8A%A4-)

---

# 1. 게임 소개

### 게임명: GhostOnly
**🎥이미지를 누르면 홍보 영상으로 이동합니다.🎥**
[![Video Label](https://github.com/jungbosong/GhostOnly/blob/YJY_README/ReadMe/main.png)](https://www.youtube.com/watch?v=80ByDIkzFlA)

### 장르 : 실시간 전략 디펜스 (싱글 플레이)

플레이어는 유령이 되어 해골을 조종해 영웅을 물리치며, 제단에 영혼을 바쳐 고대 악마님을 부활시켜야 하는 게임입니다.
*자세한 설명은 게임 설명서 참고*

### 스토리 [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)

```
평화로운 나날.
나는 오늘도 고대 악마를 부활시키기 위해 영혼을 수집하고 있다.
하지만 영웅들이 나를 가만히 둘리가 없다.
해골들과 함께 방해하는 영웅들을 제쳐두고 고대 악마님을 부활시켜 혼돈을 불러오자 !
```

### 프로젝트 목표 [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)

- **스팀 출시**
    - **개발 기간**: `2023.10.23 ~ 2023.12.15`

### 프로젝트 개요 [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)

***보다 자세한 내용은 게임 설명서를 참고해 주세요.***

---

# 2. 팀원 소개 및 개발 기간 [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)

## 2-1.  팀원 소개

| 팀원명 | 담당 기능 | GitHub | Blog |
| --- | --- | --- | --- |
|윤지연(리더)| 게임UI, 국제화, 버전 관리, 데이터 암호화, 시간 흐름, 마도서, 관:상점 | https://github.com/jungbosong | https://while0night.tistory.com/ |
|김민상(부리더)| 애니메이션, 디자인, 게임UI, 시간흐름, 상호작용, 제단, 유령(플레이어), 해골, 영웅-침입, 장비, 게임 목표(로직), InputAciton | https://github.com/berylstar | https://velog.io/@berylstar |
|강성호| 인트로, 게임 결과창, 사운드, 묘비: 영혼 수확, 장비, 미니맵, DOTween, 게임 목표(로직) | https://github.com/tjdgh7419 | https://tjdgh7419.tistory.com/ |
|김대열| 로비, 설정, 국제화, 버전 관리, 시간 흐름, 사운드, 해골, 해골-특성, 해골-자동화, 해골-상태창 | https://github.com/Kim-dae-yeol?tab=repositories | https://velog.io/@elmo7180 |
|조범준| 로비, 설정, 튜토리얼, 사운드, 영웅, 유령의 영지, DOTween, NavMesh | https://github.com/KimMaYa1 | https://beomjun-develop.tistory.com/ |

## 2-2. 개발 기간 [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)

| 기간 | 작업 목록 |
| --- | --- |
| 1W 2023.10.23 ~ 2023.10.29 | • 기획<br> • 필요한 기능 정리 및 역할 분담<br> • 프로토타입 제작 |
| 2W 2023.10.30 ~ 2023.11.05 | • 프로토타입 피드백 및 반영<br> • 프로토타입 리팩토링 |
| 3W 2023.11.06 ~ 2023.11.12 | • 데모버전 제작<br> • 데모버전 피드백 |
| 4W 2023.11.13 ~ 2023.11.19 | • 중간 발표 및 피드백 반영<br> • 데모버전 디버깅<br> • 데모버전 리팩토링 |
| 5W 2023.11.20 ~ 2023.11.26 | • 튜토리얼 제작<br> • 영어버전 제작<br> • 밸런스 패치 |
| 6w 2023.11.27 ~ 2023.12.03 | • 알파테스트<br> • FGT(퍼커스그룹테스트)<br> • 밸런스 패치 |
| 7w 2023.12.04 ~ 2023.12.10 | • 베타테스트<br> • 밸런스 패치 |
| 8w 2023.12.11 ~ 2023.12.15 | • 밸런스 패치<br> • 리드미 파일 작성<br> • 최종 발표 |

---

# 3. 기능별 클래스 설명 [🔝](https://github.com/jungbosong/GhostOnly/tree/YJY_README)

- 🌐 **Util** 
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 🖱️ **UI** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    | UI_Base |  |
    | UI_Popup |  |
  
- 💽 **데이터 관리** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- **🇺🇲 국제화(I18n)** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 🤖 **상태머신** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 👻 **유령(플레이어)** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 💀 **해골** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 🦸 **영웅** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |

- ⚔️ **장비(무기)** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)    
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 🏛️ **제단** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 🪦 **묘비** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- 📜 **마도서(연구소)** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- ⚰️ **관(상점)** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- ‼️ **특성** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- ❓ **튜토리얼** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |
  
- ⚙️ **설정** [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)
    | 클래스명 | 설명 |
    | --- | --- |
    |  |  |
    |  |  |

---

# 4. 사용한 기술 목록 [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)

- **프레임 워크 & 언어**
    - .Net 2.0
    - C#
- **게임 엔진**
    - Unity - 2022.3.2f1
- **버전 관리**
    - GitHub
- **개발 환경**
    - **IDE**
        - VisualStudio
        - Rider: JetBrains
    - **OS**
        - Window 10
        - MacOS
- **데이터 관리**
    - Google Spreadsheet
- **디자인**
    - piskel

---

# 5. 트러블 슈팅 [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)

| 트러블 내용 | 해결 방법 | 해당 기술을 사용한 이유 |
| --- | --- | --- |
| 라운드 시작 후 적이 많이 소환되지 않더라도 **FPS가 5까지 현저히 떨어지는 현상**이 있었습니다. | 과도하게 사용된 Collider를 제거하고 서로 간의 거리 계산하는 타겟 매니저를 구현하였습니다. | 공격할 대상을 감지하기 위해 Collider를 이용한 충돌 감지는 비효율적으로 게임의 성능을 낮췄기 때문에, Collider를 제거하고 영웅과 해골의 **적 감지 방식을 서로 간의 거리를 계산**하는 방식으로 변경했습니다. |
| 게임 내 장비를 구현하면서 **같은 동작을 여러 사람이 구현**하면서 같은 기능을 하는 코드가 많아졌기 때문에 가독성이 떨어지는 문제가 발생했고, 추후 수정 사항이 생기면 여러 군데를 수정해야 하는 문제가 있었습니다. | **컴포지션 패턴을 도입**해 액션마다 모듈화를 진행했습니다. | 장비를 구현하는 것이 아닌 **액션을 구현해 모듈화**하고, 이를 부착함으로써 독립적인 기능을 갖춘 액션을 구현할 수 있었습니다.<br> 또한 **중복 코드의 감소로 유지 보수성과 가독성을 높일 수 있었습니다.** |
| 데이터 통신 부분에서 **불필요한 보일러 플레이트 코드를 작성**해야 한다는 문제가 있었습니다.  | HTTP 통신의 **요청과 응답을 추상화**해서 전과 대비해서 적은 코드로 데이터를 다운로드할 수 있도록 변경하였습니다.  또한 다운로드할 때 각 멤버 변수에 일일이 넣어주던 방법에서 **리플렉션을 통해 자동으로 적용**하도록 수정했습니다. 이 두 가지를 통해서 **코드의 라인 수가 600줄에서 300줄로 감소**했습니다.  | 데이터 처리 및 리플렉션을 통해서 일어나는 메모리의 회수 속도가 느릴 수 있다는 단점을 충분히 상쇄시킬 만큼 코드 라인이 극적으로 감소하여 품질이 높아지기 때문에 적용했습니다.  |
| 낮과 밤을 담당하는 스크립트에서 너무 많은 기능을 담당하는 문제가 있었습니다. | 실행되야 하는 메소드를 **델리게이트를 이용**해, 낮과 밤이 바뀔 때 **구독된 이벤트들을 실행**했습니다. | 게임 특성상 낮과 밤이 바뀔 때 변경해야 하는 점이 많았습니다. 기존에는 한 스크립트에서 몰아서 메소드를 실행해 주었기 때문에, 가독성이 떨어지고 코드 간의 결합도가 높아졌습니다. 따라서 이를 분리하기 위해 **델리게이트를 이용해 실행할 메소드들을 구독 시켜 주었습니다.**  |
| 영웅이 제단과 해골에 데미지를 입히기 위해 제단에 스크립트를 부착했지만, 대부분의 기능은 사용하지 않았던 문제가 있었습니다. | IDamagable **인터페이스를 이용**해, 다른 스크립트에 동일한 이름의 메소드를 구현하여 호출하기 용이하게 구현했습니다. | 제단과 해골이 영웅에게 피해를 입기 위해서는 체력을 담당하는 HealthSystem 스크립트를 부착해야 했습니다. 하지만, 제단에서는 HealthSystem의 대부분 기능은 제단과 연관이 없었기 때문에 필요 없는 정보들을 가지고 있었습니다. |
| 마도서에서 배운 흑마법 내용이 마도서를 껐다 다시 켜면 초기화되는 문제가 있었습니다. | 기존에 있던 UI_SpellBook 스크립트의 내용을 **MVC 패턴을 적용**해 마도서에 있는 데이터를 저장하는 SpellBookManager를 만들고, **데이터 변경을 담당하는 SpellBookController**를 만들어 **UI_SpellBook에선 UI 부분만 관리**하도록 변경해 해당 문제를 해결했습니다. | DataManager에선 마도서에 있는 내용을 초기화할 때 필요한 내용만 들고 있게 하고, SpellBookManager에선 유저가 게임을 플레이하며 바뀐 마도서 내용의 데이터를 들고 있게 기능을 분리할 수 있었기 때문입니다. 또한 기존에는 UI_SpellBook 스크립트에서 데이터 관리 및 UI 관리를 했기 때문에 **코드 결합도를 낮추기 위해 도입**했습니다. |
| Resources 폴더에서 Instantiate를 사용할 때 문자열이 반복해서 사용되는 문제가 있었습니다. | Constants 클래스를 정의해 **문자열을 상수(const)화** 시켰습니다. | Resources 폴더의 프리팹이나 이미지를 불러올 때 문자열로 주소를 받아오기 때문에 오타로 인해 오류가 날 가능성이 있었습니다. 그때 마다 어떤 스크립트에서 문자열을 사용하는지 알아야 하는데 이를 바로 인지하기가 어려웠습니다. 때문에 **Constants라는 클래스에서 사용하는 문자열을 관리**하도록 하여 **코드 가독성과 유지 보수성을 높이고자 했습니다.** |
| DOTween 기능 실행 도중 객체가 파괴될 때 DOTween 관련 오류가 있었습니다. | DOTween 기능이 다른 기능에 의해 예상치 못하게 파괴되는 부분을 찾아내어 파괴되기 전에  DOTween 기능을 먼저 삭제하는 방법으로 해결했습니다. | 게임 실행에 있어서 DOTween의 오류로 예상치 못한 버그가 발생할 수 있기 때문입니다. 이러한 이유로 오류를 모두 해결하여 게임에 버그와 같은 부정적인 영향이 가지 않게 만들었습니다. |

---

# 6. 라이선스 [🔝](https://github.com/jungbosong/GhostOnly#%EB%AA%A9%EC%B0%A8)

- **UI Particle**
    - [show-homepage](https://openupm.com/packages/com.coffee.ui-particle/#usage)
    - [show-license](https://github.com/mob-sakai/ParticleEffectForUGUI/blob/4e4b9eb2a756219cf9632525e0acb00aec30aa92/LICENSE.md?plain=1#L1-L7)
    - MIT
- **둥근모 글꼴**
    - [show-homepage](https://neodgm.dalgona.dev/index.html)
    - [show-license](https://github.com/neodgm/neodgm/blob/51aaf6e273a9bd9f9f01d647e65e572da7181128/LICENSE.txt#L1-L97)
    - SIL Open Font License 1.1
